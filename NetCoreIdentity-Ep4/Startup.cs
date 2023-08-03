using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace NetCoreIdentity_Ep4
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
            // Configure authentication services using the "CookieAuth" scheme.
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", (cookie) =>
                {
                    // Set the cookie name to "Grandmas.Cookie".
                    cookie.Cookie.Name = "Grandmas.Cookie";

                    // Set the login path to "/Home/Authenticate".
                    cookie.LoginPath = "/Home/Authenticate";
                });

            // Add authorization services and configure a custom policy named "Claim.Dob".
            services.AddAuthorization(config =>
            {
                // Define a custom authorization policy named "Claim.Dob".
                config.AddPolicy("Claim.Dob", policyBuilder =>
                {
                    // Add a requirement to the policy - users must have a claim of type "DateOfBirth".
                    // OPTION - 1
                    //policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                    
                    // OPTION - 2 using Extension method, this method is created in CustomRequiredClaim class
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            // Register the custom authorization handler for the "CustomRequireClaim" requirement as a scoped service.
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();


            services.AddControllersWithViews();
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
            }

            app.UseStaticFiles();

            app.UseRouting();

            // who are you?
            app.UseAuthentication();

            // is this user allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
