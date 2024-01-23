using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RestaurantAPI2.Authorization;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Middleware;
using RestaurantAPI2.Models;
using RestaurantAPI2.Models.Validators;
using RestaurantAPI2.Services;
using System.Reflection;
using System.Text;

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

            
            var authenticationSettings = new AuthenticationSettings();
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

            builder.Services.AddSingleton(authenticationSettings);

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", policy => 
                    policy.RequireClaim("Nationality","Polish","German"));
                options.AddPolicy("AtLeast20", policy => 
                    policy.AddRequirements(new MinimumAgeRequirements(20)));
                options.AddPolicy("AtLeast2REstaurantsCreatedByUser", 
                    policy => policy.AddRequirements(new CreateRestaurantRequirement(2)));

            });
            builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementsHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();


            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddFluentValidation(); //obsolete
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RestaurantDbContext>();

            builder.Services.AddScoped<RestaurantSeeder>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserDtoValidator>();
            builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();

            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();       
            builder.Services.AddScoped<RequestTimeMiddleware>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", policyBuilder =>
                    policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(builder.Configuration["AllowedOrigins"]));
            });
           
            var app = builder.Build();

            app.UseStaticFiles();

            app.UseCors("FrontEndClient");

            // Configure the HTTP request pipeline.

            var scoped = app.Services.CreateScope();
            var seeder = scoped.ServiceProvider.GetRequiredService<RestaurantSeeder>();
            seeder.Seed();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            //app.UseMiddleware<RequestTimeMiddleware>();

            app.UseAuthentication();
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
