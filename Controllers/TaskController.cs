using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

[ApiController]
[Route("task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService taskService;

    public TaskController(ITaskService taskService)
    {
        this.taskService = taskService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();


        var task = await taskService.CreateTaskAsync(userId, taskDto);
        var response = new TaskResponse
        {
            
            Title = taskDto.Title,
            Description = taskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = taskDto.Deadline,
            IsCompleted = taskDto.IsCompleted,
            IsPriority = taskDto.IsCompleted
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
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();

        var result = await taskService.EditTaskAsync(updateTaskDto);
        var response = new TaskResponse
        {
            Title = updateTaskDto.Title,
            Description = updateTaskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = updateTaskDto.Deadline,
            IsCompleted = updateTaskDto.IsCompleted,
            IsPriority = updateTaskDto.IsPriority
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
}