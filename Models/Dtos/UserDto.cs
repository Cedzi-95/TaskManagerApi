public class RegisterUserDto
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

}

public class RegisterUserResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Id { get; set; }

}

public class SignInUserRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class SignInUserResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}