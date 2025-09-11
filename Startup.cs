using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using platychat_dotnet.Hubs;
using platychat_dotnet.Repositories;
using platychat_dotnet.Services;
using platychat_dotnet.Settings;
using platychat_dotnet.Utils;
using platychat_dotnet.Validators;

namespace platychat_dotnet
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        // constructor setup
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }


        //configure services function
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(_configuration.GetSection("AzureAd"));

            services.AddAuthentication()
                    .AddGoogle("Google", options =>
                    {
                        options.ClientId = _configuration["GoogleAuth:ClientId"];
                        options.ClientSecret = _configuration["GoogleAuth:ClientSecret"];
                        options.CallbackPath = "/signin-google";
                    });

            services.AddControllers();
            services.AddFluentValidation(fv =>
                    {
                        fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>();
                    });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Result = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            });

            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDBSettings"));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddHttpClient<IChatAiService, ChatAiService>();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000") // React app URL
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // required for SignalR


                });
            });
        }

        // configure middlewares function
        internal void ConfigureMiddlewares(WebApplication app)
        {

            // swagger setup
            if (_environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // https setup
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            // auth setup
            app.UseAuthentication();
            app.UseAuthorization();

            // routes setup
            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");
        }
    }
}
