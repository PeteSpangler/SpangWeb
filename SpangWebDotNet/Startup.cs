using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using DbUp;
using SpangWebDotNet.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SpangWebDotNet.Authorization;

namespace SpangWebDotNet
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
            services.AddControllersWithViews();
            var connectionString =
                Configuration.GetConnectionString("DefaultConnection");
            EnsureDatabase.For.SqlDatabase(connectionString);
            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString, null)
                .WithScriptsEmbeddedInAssembly(
                System.Reflection.Assembly.GetExecutingAssembly()
                )
                .WithTransaction()
                .Build();
            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddMemoryCache();
            services.AddSingleton<IHabitCache, HabitCache>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                  JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                  JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Authority"];
                options.Audience = Configuration["Auth0:Audience"];
            });
            services.AddHttpClient();
            services.AddAuthorization(options =>
                  options.AddPolicy("MustBeQuestionAuthor", policy =>
                    policy.Requirements.Add(new MustBeHabitAuthorRequirement())));
            services.AddScoped<IAuthorizationHandler, MustBeHabitAuthorHandler>();
            services.AddHttpContextAccessor();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                  builder
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithOrigins(Configuration["Frontend"])));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
