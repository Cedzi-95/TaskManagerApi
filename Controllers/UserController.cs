using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var users = await userService.GetUsers();
            return Ok(users);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await userService.GetUserbyId(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // [HttpDelete("{userId}")]
    // public async Task<IActionResult> DeleteAsync(string userId)
    // {
    //     try
    //     {
          
    //        var deletedUser = await userService.DeleteUserAsync(userId);

    //         return Ok(new { message = "User deleted successfully" });
    //     }
    //     catch
    //     {
    //         BadRequest($"There was an issue deleting this user with id {userId}");
    //     }

    // }
}