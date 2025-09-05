using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using MongoDB.Driver;
using platychat_dotnet.Repositories;
using platychat_dotnet.Services;
using platychat_dotnet.Settings;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(_configuration.GetSection("AzureAd"));

            services.AddAuthentication()
                    .AddGoogle("Google", options =>
                    {
                        options.ClientId = "your-google-client-id";
                        options.ClientSecret = "your-google-client-secret";
                        options.CallbackPath = "/signin-google";
                    });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDBSettings"));
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

            // auth setup
            app.UseAuthentication();
            app.UseAuthorization();

            // routes setup
            app.MapControllers();
        }
    }
}
