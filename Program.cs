using Microsoft.AspNetCore.Authentication.BearerToken;
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
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=taskmanager;Username=postgres;Password=password"));

        // CORS configuration - move this BEFORE authentication/authorization
        builder.Services.AddCors(options => {
            options.AddPolicy("AllowAll", policy => {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Add Identity services
        builder.Services.AddIdentityCore<UserEntity>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

        // Configure Identity options
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });

        // FIXED: Configure Bearer Token options with longer expiration
        builder.Services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme, options =>
        {
            options.BearerTokenExpiration = TimeSpan.FromHours(2); // Set to 2 hours instead of default 1 hour
            options.RefreshTokenExpiration = TimeSpan.FromDays(14); // 14 days for refresh token
        });

        // Add authentication with bearer token
        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        builder.Services.AddAuthorization();

        // Add your services
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        // IMPORTANT: Order matters! CORS must come before authentication
        app.UseCors("AllowAll");
        
        // Don't use HTTPS redirection in development if you're using HTTP
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        // Authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map Identity API endpoints (this provides /login, /register, etc.)
        app.MapIdentityApi<UserEntity>();
        
        // Map your controllers
        app.MapControllers();

        app.Run();
    }
}