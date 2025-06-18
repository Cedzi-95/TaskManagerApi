using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    public Task<RegisterUserResponse> RegisterAsync(RegisterUserDto request);
    public Task<SignInUserResponse> LoginAsync(SignInUserRequest request);
    public Task<UserEntity> DeleteUserAsync(string userId);
    public Task<IEnumerable<UserEntity>> GetUsers();
    public Task<UserEntity> GetUserbyId(string userId);


}

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> userManager;
    private readonly SignInManager<UserEntity> signInManager;

    public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    public async Task<UserEntity> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)

            return null!;

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
            return null!;

        throw new IdentityException($"User {user.UserName} could not be deleted");
       
       
        
    }

    public async Task<UserEntity> GetUserbyId(string userId)
    {
        return await userManager.FindByIdAsync(userId)
        ?? throw new IdentityException("user not found");
    }

    public async Task<IEnumerable<UserEntity>> GetUsers()
    {
        return await userManager.Users.ToListAsync();
    }

    public async Task<SignInUserResponse> LoginAsync(SignInUserRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        if (user == null)
        {
            throw new ArgumentNullException();
        }
        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            request.Password!,
            false,
            false
        );
        if (result.Succeeded)
        {
            return new SignInUserResponse
            {
                Username = user.UserName,
                Email = user.Email
            };
        }
        throw new IdentityException("Invalid email or password");
    }


    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserDto request)
    {
        var user = new UserEntity()
        {
            UserName = request.Username,
            Email = request.Email,
            PasswordHash = request.Password
        };

        var result = await userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new IdentityException($"Error while creating user : {errorMessages}");
        }
        return new RegisterUserResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email
        };


    }
}

public class IdentityException(string message) : Exception(message) { }
