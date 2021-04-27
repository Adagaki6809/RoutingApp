using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var routeHandler = new RouteHandler(Handle);
            var routeBuilder = new RouteBuilder(app, routeHandler);
            routeBuilder.MapRoute("default", "{controller}/{action}/{id?}",
                new { action = "Index" }, // параметры по умолчанию
                new { controller = "^H.*", id = @"\d{2}" } // ограничения
            );
            app.UseRouter(routeBuilder.Build());

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private async Task Handle(HttpContext context)
        {
            var routeValues = context.GetRouteData().Values;
            var action = routeValues["controller"].ToString();
            var name = routeValues["action"].ToString();
            var year = routeValues["id"]?.ToString() ?? "";
            await context.Response.WriteAsync($"controller: {action} | action: {name} | id:{year}");
        }
    }
}
