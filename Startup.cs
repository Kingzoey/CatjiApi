﻿using CatjiApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

//Scaffold-DbContext "Data Source=localhost:1521/orcl;User Id=Catji;Password=tongji;Persist Security Info=True;" Oracle.EntityFrameworkCore -outputdir Models -f

namespace CatjiApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ModelContext>(opt => opt.UseOracle(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication("Cookies").AddCookie("Cookies", o =>
            {
                o.SlidingExpiration = true;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                o.EventsType = typeof(CustomCookieAuthenticationEvents);
            });
            services.AddScoped<CustomCookieAuthenticationEvents>();
            services.AddCors(option =>
            {
                option.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://github.io");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // using (var srvScope=app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()){
            //     var context=srvScope.ServiceProvider.GetRequiredService<>();
            //     context.Database.EnsureCreated();
            // }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseFileServer();

            app.UseCors();
            app.UseAuthentication();
        }
    }

    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly ModelContext _context;

        public CustomCookieAuthenticationEvents(ModelContext context)
        {
            _context = context;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            // Look for the LastChanged claim.
            var lastChanged = (from c in userPrincipal.Claims
                               where c.Type == "LastChanged"
                               select c.Value).FirstOrDefault();
            var usid = (from c in userPrincipal.Claims
                        where c.Type == ClaimTypes.Name
                        select c.Value).FirstOrDefault();

            var user0 = await _context.Users.FindAsync(Convert.ToInt32(usid));

            if (string.IsNullOrEmpty(lastChanged) || user0 == null ||
                user0.ChangedTime != Convert.ToDateTime(lastChanged))
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}