using Xunit;
using Moq;
using TaskManagerApi.Services;
using TaskManagerApi.Repositories;
using TaskManagerApi.Models;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Linq.Expressions;

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
        var testUserId = "1";
        var taskDto = new CreateTaskDto
        {
             Title = "Task 1",
              Description = "First task description",
              Deadline = DateTime.Parse("2025-06-25 10:29:49.221679+00"),
              IsCompleted = false,
              IsPriority = true,
        };

        _mockTaskRepository
        .Setup(repo => repo.CreateTaskAsync(It.IsAny<TaskEntity>()))
        .Returns(Task.CompletedTask);

        //Act
        var result = await _taskService.CreateTaskAsync(testUserId, taskDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(taskDto.Title, result.Title); 
        Assert.Equal(taskDto.Description, result.Description);
        Assert.Equal(testUserId, result.UserId);

        //Make sure the createTaskAsync was called once
        _mockTaskRepository.Verify(repo => repo.CreateTaskAsync(It.IsAny<TaskEntity>()), Times.Once); 
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



    [Fact]
    public async Task GetAsync_ShouldReturnTask()
    {
        //Arrange
        var testUserId = "1";
        var task = new TaskEntity
        {
            Id = 1,
              Title = "Task 1",
              Description = "First task description",
              CreateAt = DateTime.Parse("2025-06-23 10:29:49.221679+00"),
              Deadline = DateTime.Parse("2025-06-25 10:29:49.221679+00"),
              IsCompleted = false,
              IsPriority = true,
              UserId = "1"
        };
        _mockTaskRepository.
        Setup(repo => repo.GetTaskAsync(1)).ReturnsAsync(task);
        
        //Act
        var result = await _taskService.GetTaskById(testUserId, 1);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Task 1", result.Title);
        _mockTaskRepository.Verify(repo => repo.GetTaskAsync(1), Times.Once);

    }


    [Fact]
    public async Task RemoveAsync_ShouldRemoveTask()
    {
        //Arrange: mocking an entity to delete
        var testUserId = "1";
        var task = new TaskEntity
        {
            Id = 1,
              Title = "Task 1",
              Description = "First task description",
              CreateAt = DateTime.Parse("2025-06-23 10:29:49.221679+00"),
              Deadline = DateTime.Parse("2025-06-25 10:29:49.221679+00"),
              IsCompleted = false,
              IsPriority = true,
              UserId = "1"
        };
     _mockTaskRepository.
        Setup(repo => repo.GetTaskAsync(1)).ReturnsAsync(task);
        
        var result = await _taskService.GetTaskById(testUserId, 1);

        _mockTaskRepository
        .Setup(repo => repo.DeleteTaskAsync(result));

        //Act
        await _taskService.DeleteTaskAsync(testUserId, result.Id);

        //Assert
        _mockTaskRepository.Verify(repo => repo.DeleteTaskAsync(result), Times.Once);

    }



    [Fact]
    public async Task Should_CompleteTask()
    {
         //Arrange
        var testUserId = "1";
     _mockTaskRepository.
       Setup(repo => repo.CompleteTaskAsync(1, testUserId)).ReturnsAsync(true);

       //Act
       var result = await _taskService.CompleteTaskAsync(testUserId, 1);

       //Assert
       Assert.True(result);
       _mockTaskRepository.Verify(repo => repo.CompleteTaskAsync(1, testUserId), Times.Once);

    }
}