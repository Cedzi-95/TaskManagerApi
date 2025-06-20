using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
namespace TaskManagerApi;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
         // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();       

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql("Host=localhost;Database=taskmanager;Username=postgres;Password=password"));

        //in order to user ASP.NET built in tokens
        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme); 
        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddIdentityCore<UserEntity>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints()
        .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
        });
       

        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<IUserService, UserService>();


        var app = builder.Build();
        app.MapIdentityApi<UserEntity>();
        app.UseAuthorization();
        app.UseAuthorization();
        app.MapControllers();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();


        app.Run();
    }
}
