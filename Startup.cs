using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;
using Radzen;
using TacticView.Data;

namespace TacticView
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static string Token { get; set; } = null;
        public static string AccessToken { get; set; }
        public const string GITHUB_CLIENT_HEADER = "timheuer-microsoft-com";
        public static string GITHUB_CLIENT_ID { get; set; } = null;
        public static string GITHUB_CLIENT_SECRET { get; set; } = null;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<GitHubQueryService>();
            services.AddBlazoredLocalStorage();
            services.AddScoped<AppState>();
            services.AddScoped<NotificationService>();

            Token = Configuration["GITHUB_TOKEN"];
            GITHUB_CLIENT_ID = Configuration["GITHUB_CLIENT_ID"];
            GITHUB_CLIENT_SECRET = Configuration["GITHUB_CLIENT_SECRET"];
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
