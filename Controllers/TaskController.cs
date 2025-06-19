using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("task")]
public class TaskController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ITaskService taskService;

    public TaskController(IUserService userService, ITaskService taskService)
    {
        this.userService = userService;
        this.taskService = taskService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();


        var task = await taskService.CreateTaskAsync(userId, taskDto);
        return Ok(task);
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

    [HttpPut("update/{taskId}")]
    public async Task<IActionResult> UpdateTask([FromBody] int taskId, TaskDto taskDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
        var task = await taskService.EditTaskAsync(taskId, userId, taskDto);
        return Ok(task);
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