using Xunit;
using Moq;
using TaskManagerApi.Services;
using TaskManagerApi.Repositories;
using TaskManagerApi.Models;

namespace TaskManagerApiTests;

public class TaskServiceTest
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly TaskService _taskService;

    public TaskServiceTest()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _taskService = new TaskService(_mockTaskRepository.Object);
    }

[Fact]
public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        //Arrange
        var tasks = new List<TaskEntity>
        {
            new TaskEntity {Id = "1", Title = "Task 1", Description = "First task description", CreateAt = "2025-06-23 10:29:49.221679+00", Deadline = "2025-06-25 10:29:49.221679+00", IsCompleted = false, IsPriority = true, UserId = "1"},
            new TaskEntity {Id = "2", Title = "Task 2", Description = "2nd task description", CreateAt = "2025-12-23 10:29:49.221679+00", Deadline = "2026-06-25 10:29:49.221679+00", IsCompleted = false, IsPriority = true, UserId = "2"},
            new TaskEntity {Id = "3", Title = "Task 3", Description = "3rd task description", CreateAt = "2025-12-23 10:29:49.221679+00", Deadline = "2025-12-25 10:29:49.221679+00", IsCompleted = false, IsPriority = true, UserId = "3"},
            new TaskEntity {Id = "4", Title = "Task 4", Description = "4th task description", CreateAt = "2025-12-23 10:29:49.221679+00", Deadline = "2025-12-31 10:29:49.221679+00", IsCompleted = false, IsPriority = true, UserId = "4"}
    
        };

        _mockTaskRepository.Setup(repo => repo.GetTasksAsync()).ReturnsAsync(tasks);

        //Act
    var result = await _taskService.GetTasksAsync();

        //Assert
        Assert.Equal(4, result.count);
        Assert.Equal("Task 1", result[0].Title);
        _mockTaskRepository.Verify(repo => repo.GetTasksAsync, Times.Once);
    }
}