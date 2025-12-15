using Xunit;
using Moq;
using TaskManagerApi.Services;
using TaskManagerApi.Repositories;
using TaskManagerApi.Models;
using Microsoft.VisualBasic;

namespace TaskManagerApiTests;

public class TaskServiceTest
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly Mock<IUserService> _mockUserService;
    private readonly TaskService _taskService;

    public TaskServiceTest()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _mockUserService = new Mock<IUserService>();
        _taskService = new TaskService(_mockTaskRepository.Object, _mockUserService.Object);
    }


    [Fact]
    public async Task CreateTaskAsync_ShouldAddNewTask()
    {
        //Arrange: mock data to create a new task
        var newTask = new TaskEntity
        {
             Title = "Task 1",
              Description = "First task description",
              CreateAt = DateTime.Parse("2025-06-23 10:29:49.221679+00"),
              Deadline = DateTime.Parse("2025-06-25 10:29:49.221679+00"),
              IsCompleted = false,
              IsPriority = true,
              UserId = "1"
        };
        _mockTaskRepository.Setup(repo => repo.CreateTaskAsync(newTask)).ReturnsAsync(Task.CompletedTask);

        //Act
        var result = await _taskService.CreateTaskAsync(newTask);

        //Assert
        Assert.Equal(newTask, result); //Check that the result matches the mock data
        _mockTaskRepository.Verify(repo => repo.CreateTaskAsync(newTask), times.Once); //Make sure the createTaskAsync was called once
    }




    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        //Arrange
        var testUserId = "1";
        var tasks = new List<TaskEntity>
        {
            new TaskEntity{
              Id = 1,
              Title = "Task 1",
              Description = "First task description",
              CreateAt = DateTime.Parse("2025-06-23 10:29:49.221679+00"),
              Deadline = DateTime.Parse("2025-06-25 10:29:49.221679+00"),
              IsCompleted = false,
              IsPriority = true,
              UserId = "1"
              },
            new TaskEntity {
                Id = 2,
                Title = "Task 2",
                Description = "2nd task description",
                CreateAt = DateTime.Parse("2025-12-23 10:29:49.221679+00"),
                Deadline = DateTime.Parse("2026-06-25 10:29:49.221679+00"),
                IsCompleted = false,
                IsPriority = true,
                UserId = "1"
                },
            new TaskEntity {
                Id = 3,
                Title = "Task 3",
                Description = "3rd task description",
                CreateAt = DateTime.Parse("2025-12-23 10:29:49.221679+00"),
                Deadline = DateTime.Parse("2025-12-25 10:29:49.221679+00"),
                IsCompleted = false,
                IsPriority = true,
                UserId = "1"
                },
            new TaskEntity {
                Id = 3,
                Title = "Task 4",
                Description = "4th task description",
                CreateAt = DateTime.Parse("2025-12-23 10:29:49.221679+00"),
                Deadline = DateTime.Parse("2025-12-31 10:29:49.221679+00"),
                IsCompleted = false,
                IsPriority = true,
                UserId = "1"
                }
        };

        _mockTaskRepository.Setup(repo => repo.GetAllTasksAsync(testUserId)).ReturnsAsync(tasks);

        //Act
        var result = await _taskService.GetTasksAsync(testUserId);

        //Assert
        var taskList = result.ToList(); //convert IEnumerable to list in order to index
        Assert.Equal(4, taskList.Count);
        Assert.Equal("Task 1", taskList[0].Title);
        _mockTaskRepository.Verify(repo => repo.GetAllTasksAsync(testUserId), Times.Once);
    }
}