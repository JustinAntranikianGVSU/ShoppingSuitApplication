using AutoMapper;
using DataAccess;
using Domain;
using Domain.Orchestrators;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingSuitePlatform.MiddleWare;

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

			services.AddAuthentication().AddCookie(options =>
			{
				//options.AccessDeniedPath = "/account/denied";
				//options.LoginPath = "/account/login";
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

			services.AddIdentity<IdentityUser, IdentityRole>().AddUserStore<AppUserStore>().AddRoleStore<AppRoleStore>().AddDefaultTokenProviders();

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

			// Orchestrators
			services.AddScoped<ICreateUserOrchestrator, CreateUserOrchestrator>();
			services.AddScoped<IGetUserOrchestrator, GetUserOrchestrator>();
			services.AddScoped<ILoginOrchestrator, LoginOrchestrator>();
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
