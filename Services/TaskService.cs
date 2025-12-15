using TaskManagerApi.Models;
using TaskManagerApi.Repositories;

namespace TaskManagerApi.Services;
public interface ITaskService
{
    public Task<TaskEntity> CreateTaskAsync(string userId, CreateTaskDto taskDto);
    public Task<IEnumerable<TaskEntity>> GetTasksAsync(string userId);
    public Task<TaskEntity> GetTaskById(string userId, int taskId);
    public Task DeleteTaskAsync(string userId, int taskId);
    public Task<TaskEntity> EditTaskAsync(UpdateTaskDto updateTaskDto);
    public Task<bool> CompleteTaskAsync(string userId, int taskId);

}

public class TaskService : ITaskService
{
    private readonly ITaskRepository taskRepository;
    private readonly IUserService userService;

    public TaskService(ITaskRepository taskRepository, IUserService userService)
    {
        this.taskRepository = taskRepository;
        this.userService = userService;
    }

    public async Task<bool> CompleteTaskAsync(string userId, int taskId)
    {
        var task = await taskRepository.CompleteTaskAsync(taskId, userId);

        return task;
    }

    public async Task<TaskEntity> CreateTaskAsync(string userId, CreateTaskDto taskDto)
    {
        var task = new TaskEntity
        {
            Id = 0,
            Title = taskDto.Title!
            ,
            Description = taskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = taskDto.Deadline,
            IsCompleted = taskDto.IsCompleted,
            IsPriority = taskDto.IsPriority,
            UserId = userId
        };
        await taskRepository.CreateTaskAsync(task);
        return task;

    }

    public async Task DeleteTaskAsync(string userId, int taskId)
    {
        var taskEntity = await taskRepository.GetTaskAsync(taskId);
        if (taskEntity == null || taskEntity.UserId != userId)
        {
            throw new ArgumentException("task not found or you don't have access to this task.");
        }
        await taskRepository.DeleteTaskAsync(taskEntity);

    }

    public async Task<TaskEntity> EditTaskAsync(UpdateTaskDto updateTaskDto)
    {
        var UpdateTask = new TaskEntity
        {
            Id = updateTaskDto.Id,
            Title = updateTaskDto.Title!,
            Description = updateTaskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = updateTaskDto.Deadline,
            IsCompleted = updateTaskDto.IsCompleted,
            IsPriority = updateTaskDto.IsPriority

        };
        await taskRepository.EditTaskAsync(UpdateTask);
        return UpdateTask;

    }

    public async Task<TaskEntity> GetTaskById(string userId, int taskId)
    {

        var task = await taskRepository.GetTaskAsync(taskId);
        if (userId != task.UserId)
        {
            throw new ArgumentException("You can't access this task.");
        }
        return task;
    }

    public async Task<IEnumerable<TaskEntity>> GetTasksAsync(string userId)
    {
        return await taskRepository.GetAllTasksAsync(userId);
    }


}