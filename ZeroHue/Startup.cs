using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZeroHue.Services;
using ZeroHue.Services.Hubs;
using ZeroHue.Services.Repositories;


namespace ZeroHue
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSet.FrontendIP = Configuration["UPNP:FrontendIP"];
            AppSet.FrontendPort = Configuration["UPNP:FrontendPort"];
            //192.16FFFE8.1.99
            var seed = AppSet.FrontendIP.Split(".");
            seed[1] += "FFFE8";
            AppSet.Huebridgeid = string.Join(".", seed);

            CreateDescriptionFile();
        }

        private void CreateDescriptionFile()
        {
            if (!System.IO.File.Exists("./wwwroot/description.xml"))
            {
                var description = System.IO.File.ReadAllText($"{AppSet.PATH_FILES}description.xml");
                description = description.Replace("[IP]", AppSet.FrontendIP);
                System.IO.File.WriteAllText("./wwwroot/description.xml", description);
            }
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();


            services.AddScoped<INotificationService, NotificationSignalRService>();

            services.AddScoped<ILightRepository,LightRepository>();

            services.AddScoped<ILightService,LightService>();

            services.AddScoped<LightsMessageCenter>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<HueLightsHub>("/huelightshub");
            });
        }
    }
}
