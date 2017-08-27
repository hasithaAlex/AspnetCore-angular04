using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using vega.Persistence;

namespace WebApplicationBasic
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        
        
        
        
        // This method gets called by the runtime. Use this method to add services to the container.
        /****this methord Used for Dependency injection*/
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            // Add framework services.
            services.AddMvc();
            
        }
        /**********************end */
        
        
        
        
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /*****************This methord used to loggin*/    
            loggerFactory.AddConsole(Configuration.GetSection("Logging")); 
            loggerFactory.AddDebug();
            /*****************end */


            /*****************Configer request processing pipeline(only use in dev environment) */    
            if (env.IsDevelopment())/*****************If you in the developer environment*/    
            {
                /*****************Add developer exeception page*/    
                app.UseDeveloperExceptionPage();
                /*****************When you change client side files Web pack automatically 
                ******************compile that and push changes to web bowser*/
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            /*****************will be able to serve static files like images styles */    
            app.UseStaticFiles();

            /*****************according to request base on route it fowerd in to action and controller */
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            
        }
    }
}
