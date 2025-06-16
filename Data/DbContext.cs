using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<TaskEntity> Tasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) {}
}