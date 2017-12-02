using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ImVehicleCore.Services;

namespace ImVehicleMIS
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


            //services.AddDbContext<VehicleDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<VehicleDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ReleaseConnection")));
            //services.AddDbContext<VehicleDbContext>(options =>
            // options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));


            services.AddIdentity<VehicleUser,VehicleRole>()
                .AddEntityFrameworkStores<VehicleDbContext>()
                .AddDefaultTokenProviders();
         
            services.AddMvc().AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Vehicle");
                    options.Conventions.AuthorizeFolder("/Driver");
                    options.Conventions.AuthorizeFolder("/Group");
                    options.Conventions.AuthorizeFolder("/Town");
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireTownManagerRole", policy => policy.RequireRole("TownManager"));
                options.AddPolicy("RequireAdminsRole", policy => policy.RequireRole("Admins"));
                options.AddPolicy("RequireGlobalVisitorRole", policy => policy.RequireRole("GlobalVisitor"));
            });

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));



            services.AddScoped<ITownRepository,TownRepository>();
            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<INewsService, NewsService>();
            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseStaticFiles();

           



            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        
        }
    }
}
