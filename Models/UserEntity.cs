using Microsoft.AspNetCore.Identity;

public class UserEntity : IdentityUser
{
    public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}
