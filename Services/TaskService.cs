public interface ITaskService
{
    public Task<TaskEntity> CreateTaskAsync(string userId, TaskDto taskDto);
    public Task<IEnumerable<TaskEntity>> GetTasksAsync(string userId);
    public Task<TaskEntity> GetTaskById(string userId, int taskId);
    public Task DeleteTaskAsync(string userId, int taskId);
    public Task<TaskEntity> EditTaskAsync( TaskDto taskDto);

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
    public async Task<TaskEntity> CreateTaskAsync(string userId, TaskDto taskDto)
    {
        var user = await userService.GetUserbyId(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        var task = new TaskEntity
        {
            Id = 0,
            Title = taskDto.Title,
            Description = taskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = taskDto.Deadline,
            IsCompleted = taskDto.IsCompleted,
            IsPriority = taskDto.IsPriority
        };
        await taskRepository.CreateTaskAsync(task);
        return task;
         
    }

    public async Task DeleteTaskAsync(string userId, int taskId)
    {
        var taskEntity = await taskRepository.GetTaskAsync(taskId);
        if (taskEntity == null)
        {
            throw new ArgumentException("task not found");
        }
         await taskRepository.DeleteTaskAsync(taskEntity);
        
    }

    public async Task<TaskEntity> EditTaskAsync(TaskDto taskDto)
    {
        // var user = await userService.GetUserbyId(userId);
        // if (user == null)
        // {
        //     throw new ArgumentException("User not found");
        // }
        // var task = await taskRepository.GetTaskAsync(taskId);
        // if (task.Id != taskId)
        // {
        //     throw new ArgumentException("No such task id was found");
        // }

        var UpdateTask = new TaskEntity
        {

            Title = taskDto.Title,
            Description = taskDto.Description,
            CreateAt = DateTime.UtcNow,
            Deadline = taskDto.Deadline,
            IsCompleted = taskDto.IsCompleted,
            IsPriority = taskDto.IsPriority
        };
        await taskRepository.EditTaskAsync(UpdateTask);
        return UpdateTask;
         
    }

    public async Task<TaskEntity> GetTaskById(string userId, int taskId)
    {
        return await taskRepository.GetTaskAsync(taskId);
    }

    public async Task<IEnumerable<TaskEntity>> GetTasksAsync(string userId)
    {
        return await taskRepository.GetAllTasksAsync(userId);
    }
}