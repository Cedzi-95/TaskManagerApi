public interface ITaskService
{
    public Task<TaskEntity> CreateTaskAsync(string userId, TaskDto taskDto);
    public Task<IEnumerable<TaskEntity>> GetTasksAsync(string userId);
    public Task<TaskEntity> GetTaskById(string userId, int taskId);
    public Task DeleteTaskAsync(string userId);
    public Task EditTaskAsync(string userId);

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

    public Task DeleteTaskAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task EditTaskAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskEntity> GetTaskById(string userId, int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskEntity>> GetTasksAsync(string userId)
    {
        throw new NotImplementedException();
    }
}