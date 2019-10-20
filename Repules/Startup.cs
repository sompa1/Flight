using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repules.Dal;
using Repules.Bll.Managers;
using Repules.Model;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using System.Threading;

namespace Repules
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true                   
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            services.AddBll();

            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
              .AddEntityFrameworkStores<ApplicationContext>()
              .AddDefaultTokenProviders();

            services.AddMvc(config =>
            {
                // using Microsoft.AspNetCore.Mvc.Authorization;
                // using Microsoft.AspNetCore.Authorization;
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
       .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // var connection = @"Server=(localdb)\mssqllocaldb;Database=Repules;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContext<ApplicationContext>
            // (options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IHostingEnvironment env, FlightManager flightManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseStaticFiles();

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate(
                () => flightManager.ProcessFlightsAsync(),
                Cron.MinuteInterval(2));

            app.UseAuthentication(); //autentikációs middleware

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //SetupAuth(app.ApplicationServices).Wait();
        }

        public static async Task SetupAuth(IServiceProvider serviceProvider) //beszurja az adatbazisba a felhasznalókat és a role-okat
        {
            using (var scope = serviceProvider.CreateScope())
            {
                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                string username = "admin";
                string email = "admin@admin.hu";
                string password = "TestAdmin123";

                if (await roleManager.FindByNameAsync("admin") == null) //ha meg nincs admin role, akkor adja hozzá az admint
                    await roleManager.CreateAsync(new ApplicationRole("admin"));


                if (await roleManager.FindByNameAsync("user") == null)
                    await roleManager.CreateAsync(new ApplicationRole("user"));

                if (await userManager.FindByNameAsync(username) == null)
                {
                    ApplicationUser user = new ApplicationUser { UserName = username, Email = email };
                    IdentityResult result = await userManager.CreateAsync(user, password); //ha nem sikerult letrehozni az új usert
                    if (!result.Succeeded)
                        throw new Exception("Could not create admin user");
                    await userManager.AddToRoleAsync(user, "admin"); //beallitjuk a szerepkörét
                }
            }
        }


    }
}
