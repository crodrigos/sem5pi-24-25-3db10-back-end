using dddnet8;
using dddnet8.Infraestructure;
using System.Net;


var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 7081);
});

builder.Services.AddHostedService<PatientCleanupService>();

// Carregar a configuração
var appSettingsBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();


var startup = new Startup(appSettingsBuilder);

startup.ConfigureServices(builder.Services);


var app = builder.Build();

builder.Logging.AddConsole();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var bootstrapper = services.GetRequiredService<ApplicationBootstrapper>();
    await bootstrapper.Initialize();
}


startup.Configure(app);

app.Run();