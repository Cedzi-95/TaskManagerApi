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
    private readonly ILogger<TaskController> _logger;


    public TaskController(ITaskService taskService, IUserService userService, ILogger<TaskController> _logger)
    {
        this.taskService = taskService;
        this.userService = userService;
        this._logger = _logger;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto taskDto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var user = await userService.GetUserbyId(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, "An error occurred while creating the task");
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllTaskAsync()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var tasks = await taskService.GetTasksAsync(userId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
            return StatusCode(500, "An error occurred while retrieving tasks");
        }
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskByIdAsync(int taskId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var task = await taskService.GetTaskById(userId, taskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task {TaskId}", taskId);
            return StatusCode(500, "An error occurred while retrieving the task");
        }

    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var user = await userService.GetUserbyId(updateTaskDto.UserId!);
            if (user == null)
            {
                return NotFound("User not found");
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
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task");
            return StatusCode(500, "An error occurred while updating the task");
        }
    }

    [HttpDelete("{taskId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTaskAsync(int taskId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var task = await taskService.GetTaskById(userId, taskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            await taskService.DeleteTaskAsync(userId, taskId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
            return StatusCode(500, "An error occurred while deleting the task");
        }
    }

    [HttpPatch("complete")]
    [Authorize]
    public async Task<IActionResult> CompleteTaskAsync([FromBody] CompleteTaskDto request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing task {TaskId}", request.Id);
            return StatusCode(500, "An error occurred while completing the task");
        }

    }



}