﻿using JwtAuth.Infrastructure.IoC;
using JwtAuth.Infrastructure.Settings;
using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JwtAuth.Infrastructure.Services;

namespace JwtAuth.Api
{
	public class Startup
	{
		public IConfigurationRoot Configuration { get; }
		public IContainer ApplicationContainer { get; private set; }

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddAuthorization(x => x.AddPolicy("admin", p => p.RequireRole("admin")));
			services.AddMemoryCache();
			services.AddMvc()
				   .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);

			var builder = new ContainerBuilder();
			builder.Populate(services);
			builder.RegisterModule(new ContainerModule(Configuration));
			ApplicationContainer = builder.Build();

			return new AutofacServiceProvider(ApplicationContainer);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			var jwtSettings = app.ApplicationServices.GetService<JwtSettings>();
			app.UseJwtBearerAuthentication(new JwtBearerOptions
			{
				AutomaticAuthenticate = true,
				TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = "http://localhost:5000",
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
				}
			});

			var generalSettings = app.ApplicationServices.GetService<GeneralSettings>();
			if (generalSettings.SeedData)
			{
				var dataInitializer = app.ApplicationServices.GetService<IDataInitializer>();
				dataInitializer.SeedAsync();
			}
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseMvc();
		}
	}
}
