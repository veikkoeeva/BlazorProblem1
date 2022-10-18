using BlazorApp1.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp1.Server
{
    public static class Program
    {
        public static WebApplication InternalMain(WebApplication app)
        {
            if(app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseDeveloperExceptionPage();

                //app.UseSwagger();
                //app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //Why this works when called from ApiTests?
            app.MapGet("/WeatherForecast", async context =>
            {
                await context.Response.WriteAsJsonAsync<List<WeatherForecast>>(new List<WeatherForecast>(new[] { new WeatherForecast() }));
            });

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            //This also works when called from ApiTests.
            /*
            app.Use(async (context, next) =>
            {
                if(context.Request.Path == "/WeatherForecast")
                {
                    await context.Response.WriteAsJsonAsync<List<WeatherForecast>>(new List<WeatherForecast>(new[] { new WeatherForecast() }));
                    return;
                }
                                
                await next(context);
            });*/

            return app;
        }

        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            var app = builder.Build();

            InternalMain(app).Run();
        }

        public static WebApplicationBuilder CreateWebHostBuilder(string[] args)
        {

            var builder = WebApplication.CreateBuilder();

            string environmentName = builder.Environment.EnvironmentName;

            builder.Configuration.AddJsonFile("appsettings.Server.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile($"appsettings.Server.{environmentName}.json", optional: false, reloadOnChange: true);
            
            //builder.Services.AddSwaggerGen();
            builder.Services.AddControllersWithViews();
            builder.Services
                .AddProblemDetails()
                .AddMvcCore()
                .AddApiExplorer()
                .AddDataAnnotations()
                .AddFormatterMappings()
                .AddCacheTagHelper()
                .AddViews()
                .AddRazorViewEngine()
                .AddRazorPages();

            //The port here is hardcoded to make the demonstration purpose simpiler.
            //Otherwise it would be a random port as parameters.
            _ = builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(7001, configure => configure.UseHttps());
            });

            return builder;
        }
    }
}
