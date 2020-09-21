using BrainHi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrainHi.WebApp
{
    public class Startup
    {
        public IConfiguration Config { get; set; }

        public Startup(IConfiguration config) => Config = config;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * IF YOU HAVE A SQL DATABASE FOR TESTING THEN
             *  1. UNCOMMENT THE NEXT TWO LINE, 
             *  2. MAKE SURE THAT THE 'appsetting.json' FILE CONTAINS THE CORRECT CONNECTION STRING TO THE DATABASE 
             *  3. AND COMMENT THE FOLLOWING LINE: services.AddSingleton<IApplicationRepository, MemoryApplicationRepository>();
             */
            //  Add DB Context, and Add Db context as inteface
            //services.AddDbContext<BrainHiContext>(options => options.UseSqlServer(Config.GetConnectionString("DefaultConnection")));
            //services.AddScoped<IApplicationRepository, ApplicationRepository>();

            //  IF YOU DON'T HAVE A SQL DATABASE, then use the Memory repository that will keep the objects in memory for testing (not suitable for production)
            services.AddSingleton<IApplicationRepository, MemoryApplicationRepository>();

            //  Add MVC services
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
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
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/not-found");
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
