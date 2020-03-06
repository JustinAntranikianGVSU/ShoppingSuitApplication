using AutoMapper;
using DataAccess;
using Domain;
using Domain.Orchestrators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ShoppingSuitePlatform.MiddleWare;
using System;
using System.Text;
using ShoppingSuitePlatform.Helpers;
using CoreLibrary;

namespace ShoppingSuitePlatform
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
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.Events = new JwtBearerEvents
				{
					OnTokenValidated = async (ctx) =>
					{
						var helper = new JwtRequestContextHelper(ctx);
						helper.InitContext();
					}
				};

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["Jwt:Issuer"],
					ValidAudience = Configuration["Jwt:Issuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
				};
			});

			services.AddAuthorization(options =>
			{
				AppPolicy.GetPolicyToPermissionMappings().ForEach(oo =>
				{
					var (policyName, permission) = oo;
					var requirement = new PermissionRequirement(permission);
					options.AddPolicy(policyName, policy => policy.Requirements.Add(requirement));
				});
			});

			services.AddControllersWithViews();
			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});

			services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

			var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
			services.AddSingleton(mappingConfig.CreateMapper());

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

			services.AddScoped(BuildJwtUserContext);

			// Orchestrators
			services.AddScoped<ICreateUserOrchestrator, CreateUserOrchestrator>();
			services.AddScoped<IGetLocationsByUserOrchestrator, GetLocationsByUserOrchestrator>();
			services.AddScoped<IGetLocationsOrchestrator, GetLocationsOrchestrator>();
			services.AddScoped<IGetUserOrchestrator, GetUserOrchestrator>();
			services.AddScoped<ILoginOrchestrator, LoginOrchestrator>();
			services.AddScoped<IMyProfileOrchestrator, MyProfileOrchestrator>();
			services.AddScoped<IAccessListOrchestrator, AccessListOrchestrator>();
			services.AddScoped<IGetUsersByLocationOrchestrator, GetUsersByLocationOrchestrator>();
			services.AddScoped<IImpersonateOrchestrator, ImpersonateOrchestrator>();
		}

		public static JwtRequestContext BuildJwtUserContext(IServiceProvider serviceProvider)
		{
			return new JwtRequestContext
			{
				LoggedInUserId = -1
			};
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
			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});
		}
	}
}
