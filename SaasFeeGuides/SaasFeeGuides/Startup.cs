using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using SaasFeeGuides.Data;
using SaasFeeGuides.Extensions;
using SaasFeeGuides.Models.Entities;
using SaasFeeGuides.Models;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SaasFeeGuides.Helpers;
using Microsoft.AspNetCore.Authentication;
using SaasFeeGuides.Auth;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;

namespace SaasFeeGuides
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkm"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }

        public static string[] CommandLineArguments { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            SaasFeeGuides.Database.Deploy.Program.DeployDatabase(connectionString);
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {

                options.UseSqlServer(connectionString,
            b => b.MigrationsAssembly("SaasFeeGuides")).UseLazyLoadingProxies(false);
            });
           
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSingleton<IJwtFactory, JwtFactory>();

            // jwt wire up
            // Get options from app settings

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
          
            //Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication((options) =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          
            }).AddJwtBearer((options) =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                foreach(var policy in AuthorizationPolicies.Policies)
                {
                    options.AddPolicy(policy.Key, p =>
                    {
                        if (policy.Any(x => x.values.Any()))
                        {
                            p.AddAuthenticationSchemes("Bearer");
                        }
                        foreach (var (role, values) in policy)
                        {
                            p.RequireClaim(role,values);
                        }
                    });
                }

                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                   .RequireAuthenticatedUser()
                   .AddAuthenticationSchemes("Bearer")
                   .Build();

               
            });
 
            services.AddIdentity<AppUser, IdentityRole>
                (o =>
                {
                    // configure identity options
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IActivityRepository>(new ActivityRepository(connectionString));
            services.AddSingleton<ICustomerRepository>(new CustomerRepository(connectionString));
            services.AddSingleton<IContentRepository>(new ContentRepository(connectionString));
            services.AddSingleton<IEquiptmentRepository>(new EquiptmentRepository(connectionString));
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (IsDevelopment(env))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

         

            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if (error != null)
                      {
                          context.Response.AddApplicationError(error.Error.Message);
                          await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                      }
                  });
            });

            var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-UK"),
                    new CultureInfo("de-CH")
                };

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,

            };

            app.UseRequestLocalization(options);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMvc();
            

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (IsDevelopment(env))
                {
                    spa.Options.SourcePath = @"ClientApp";

                    spa.UseAngularCliServer(npmScript: "start");

                }
            });
        }

        protected virtual bool IsDevelopment(IHostingEnvironment env)
        {
            return env.IsDevelopment();
        }
    }
}
