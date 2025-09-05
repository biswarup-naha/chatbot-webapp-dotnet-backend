using platychat_dotnet;

var builder = WebApplication.CreateBuilder(args);

// Delegate all logic to Startup
var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.ConfigureMiddlewares(app);

app.Run();
