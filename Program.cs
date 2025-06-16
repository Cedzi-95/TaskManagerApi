using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace TaskManagerApi;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql("Host=localhost;Database=taskmanager;Username=postgres;Password=password"));

        //in order to user ASP.NET built in tokens
        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme); 
        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddIdentityCore<UserEntity>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();
        app.MapIdentityApi<UserEntity>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthorization();


        app.Run();
    }
}
