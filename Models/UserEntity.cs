using Microsoft.AspNetCore.Identity;
using TaskManagerApi.Models;

public class UserEntity : IdentityUser
{
    public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}
