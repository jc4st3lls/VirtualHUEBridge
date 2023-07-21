using System.Security.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZeroHue;
using ZeroHue.Services;
using ZeroHue.Services.Hubs;
using ZeroHue.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);



var _logfac = LoggerFactory.Create(configure => {
    configure.AddConsole();
});
var _log = _logfac.CreateLogger("Configuration");

_log.LogInformation($"ZeroHue");

AppSet.ApiIP = builder.Configuration["Api:IP"];
AppSet.ApiPort = builder.Configuration["Api:Port"];
AppSet.FrontendIP = builder.Configuration["UPNP:FrontendIP"];
AppSet.FrontendPort = builder.Configuration["UPNP:FrontendPort"];
//192.16FFFE8.1.99
var seed = AppSet.FrontendIP.Split(".");
seed[1] += "FFFE8";
AppSet.Huebridgeid = string.Join(".", seed);

CreateDescriptionFile();


builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(80);
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;

        listenOptions.UseHttps(AppSet.GetSelfSignedCertificate(), options =>
        {
            options.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
        });
    });
});



// Add services to the container.
builder.Services.AddScoped<INotificationService, NotificationSignalRService>();

builder.Services.AddScoped<ILightRepository, LightRepository>();

builder.Services.AddScoped<ILightService, LightService>();

builder.Services.AddScoped<LightsMessageCenter>();

builder.Services.AddSignalR();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<HueLightsHub>("/huelightshub");

app.MapGet("/", () => "ZeroHue is Up!!!");

app.Run();


void CreateDescriptionFile()
{
    //if (!System.IO.File.Exists("./wwwroot/description.xml"))
    //{
    var description = System.IO.File.ReadAllText($"{AppSet.PATH_FILES}description.xml");
    description = description.Replace("[IP]", AppSet.ApiIP);
    description = description.Replace("[PORT]", AppSet.ApiPort);
    System.IO.File.WriteAllText("./wwwroot/description.xml", description);
    //}

}