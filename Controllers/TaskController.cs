using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

[ApiController]
[Route("task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService taskService;
    private readonly IUserService userService;
    

    public TaskController(ITaskService taskService, IUserService userService)
    {
        this.taskService = taskService;
        this.userService = userService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto taskDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
        var user = await userService.GetUserbyId(userId);



        var task = await taskService.CreateTaskAsync(userId, taskDto);
        var response = new CreateTaskResponse
        {

            Title = taskDto.Title,
            Description = taskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = taskDto.Deadline,
            IsCompleted = taskDto.IsCompleted,
            IsPriority = taskDto.IsPriority,
            CreatedBy = user.UserName
        };
        return Created($"Created new task Id {task.Id}", response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTaskAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();


        var tasks = await taskService.GetTasksAsync(userId);
        return Ok(tasks);
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskByIdAsync(int taskId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
        var task = await taskService.GetTaskById(userId, taskId);
        return Ok(task);

    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskDto updateTaskDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
        var user = await userService.GetUserbyId(updateTaskDto.UserId!);
        if (user == null)
        {
            return NotFound();
        }


        var result = await taskService.EditTaskAsync(updateTaskDto);
        var response = new UpdateTaskResponse
        {
            Title = updateTaskDto.Title,
            Description = updateTaskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = updateTaskDto.Deadline,
            IsCompleted = updateTaskDto.IsCompleted,
            IsPriority = updateTaskDto.IsPriority,
            UpdatedBy = user.UserName
        };
        return Created($"Task '{result.Title}' has been modified: ", response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTaskAsync(int taskId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
        var task = await taskService.GetTaskById(userId, taskId);
        if (task == null)
        {
            return NotFound();
        }
        await taskService.DeleteTaskAsync(userId, taskId);
        return NoContent();
    }

    [HttpPatch("complete")]
    [Authorize]
    public async Task<IActionResult> CompleteTaskAsync([FromBody] CompleteTaskDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();     

         var task = await taskService.GetTaskById(userId, request.Id);
    if (task == null)
    {
        return NotFound("Task not found or doesn't belong to user");
    }   

        var result = await taskService.CompleteTaskAsync(userId, request.Id);

        if (result)
        {
            return Ok(new { message = $"{task.Title} has been successfully completed" });
        }
        return BadRequest($"Task {task.Title} could not be completed");
    }


    
}