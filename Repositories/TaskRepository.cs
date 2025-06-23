using Microsoft.EntityFrameworkCore;

public interface ITaskRepository
{
    public Task CreateTaskAsync(TaskEntity taskEntity);
    public Task<TaskEntity> GetTaskAsync(int taskId);
    public Task<IEnumerable<TaskEntity>> GetAllTasksAsync(string userId);
    public Task EditTaskAsync(TaskEntity taskEntity);
    public Task DeleteTaskAsync(TaskEntity taskEntity);
    public  Task<bool> CompleteTaskAsync(int taskId, string userId);
}

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext context;

    public TaskRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task CreateTaskAsync(TaskEntity taskEntity)
    {
        await context.Tasks.AddAsync(taskEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(TaskEntity taskEntity)
    {
        context.Tasks.Remove(taskEntity);
        await context.SaveChangesAsync();
    }

    public async Task EditTaskAsync(TaskEntity taskEntity)
    {
        context.Entry(taskEntity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync(string userId)
    {
        var tasks = await context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        return tasks;
    }

    public async Task<TaskEntity> GetTaskAsync(int taskId)
    {
        return await context.Tasks.FindAsync(taskId) ?? throw new ArgumentException("task not found");
    }

    public async Task<bool> CompleteTaskAsync(int taskId, string userId)
    {
        var task = await context.Tasks.Where(t => t.Id == taskId && t.UserId == userId)
        .FirstOrDefaultAsync();

        if (task != null)
        {
            task.IsCompleted = true;
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    
    
}