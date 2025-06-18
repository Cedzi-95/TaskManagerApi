using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TaskController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ITaskService taskService;

    public TaskController(IUserService userService, ITaskService taskService)
    {
        this.userService = userService;
        this.taskService = taskService;
    }

    
}