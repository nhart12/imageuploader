using FluentValidation.AspNetCore;
using ImageUploader.Core.Models;
using ImageUploader.Internal.Public;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImageUploader
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ImageDtoValidator>())
                     ;


            services.AddCors((options =>
            {
                options.AddPolicy("Local", builder => builder
                            .WithOrigins("http://localhost:4200", "Access-Control-Allow-Origin")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                 );
            }));
            services.AddInternalServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Local");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}
