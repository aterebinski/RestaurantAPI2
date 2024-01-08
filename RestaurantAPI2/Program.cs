using Microsoft.AspNetCore.Identity;
using NLog.Web;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Middleware;
using RestaurantAPI2.Services;
using System.Reflection;

namespace RestaurantAPI2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();       
            builder.Services.AddScoped<RequestTimeMiddleware>();
           
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            var scoped = app.Services.CreateScope();
            var seeder = scoped.ServiceProvider.GetRequiredService<RestaurantSeeder>();
            seeder.Seed();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            //app.UseMiddleware<RequestTimeMiddleware>();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
