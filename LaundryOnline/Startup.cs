using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LaundryOnline.Models;
using NToastNotify;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rotativa.AspNetCore;

namespace LaundryOnline
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
            services.AddDbContext<LaundryOnlineContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("LaundryOnlineContext")));
            services.AddDistributedMemoryCache();
            services.AddAuthentication("BkapSecuritySchema").AddCookie("BkapSecuritySchema", ops =>
            {
                ops.Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    HttpOnly = true,
                    Name = "Bkap.Security.Cookie",
                    Path = "/",
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                    SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest
                };
                ops.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Admin/Login/Index");
                ops.ReturnUrlParameter = "RequestPath";
                ops.SlidingExpiration = true;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().AddNToastNotifyNoty(new NotyOptions
            {
                ProgressBar = true,
                Timeout = 1000,
                Theme = "metroui",
                Layout = "topRight",
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = new TimeSpan(0, 30, 0);
                options.Cookie.Name = "Bkap.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseNToastNotify();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "admin",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
              );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
            RotativaConfiguration.Setup(env);
        }
    }
}
