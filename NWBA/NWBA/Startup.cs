using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NWBA.Data;
using BusinessLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading;

namespace NWBA
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
            services.AddDbContext<NwbaContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(NwbaContext)));

                // Enable lazy loading.
                options.UseLazyLoadingProxies();
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential.
                options.Cookie.IsEssential = true;
            });

            //Auto logout cookie settings
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Login";
                options.Cookie.Name = "NWBAAdminCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
                //options.LogoutPath = "/SecureLogin/LogoutNow";
                options.LoginPath = "/Login";
                options.ReturnUrlParameter = "/Login/LogoutExpired";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            //services.AddSingleton<BillPayScheduler>();
            services.AddHostedService<BillPayScheduler>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }

            //set up a middleware to handle the request in the pipeline
            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            //app.ApplicationServices.GetService<BillPayScheduler>().StartAsync(new CancellationToken());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
