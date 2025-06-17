public interface ITaskService
{
    public Task CreateTaskAsync(string userId);
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
    public Task CreateTaskAsync(string userId)
    {
        throw new NotImplementedException();
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