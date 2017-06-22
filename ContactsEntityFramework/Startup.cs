using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;                                   // added
using ContactsEntityFramework.Models;                                       // added 
using Microsoft.EntityFrameworkCore;                                        // added

namespace ContactsEntityFramework
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }       // added

        public Startup(IHostingEnvironment env)                             // added
        {
            var builder = new ConfigurationBuilder();
            var config = builder.SetBasePath(env.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: false)
                                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)   // added
                                .AddEnvironmentVariables();                                                 // added 
            Configuration = config.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();                                                      // added

            var connection = Configuration["ConnectionStrings:DefaultConnection"];  // added

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ContactsContext>(options =>
                {
                    options.UseSqlServer(connection);
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();                               // added
            
            app.UseStaticFiles();                                       // added

            app.UseMvc(routes =>                                        // added
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
