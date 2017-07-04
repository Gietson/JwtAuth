using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using JwtAuth.Infrastructure.Mappers;
using JwtAuth.Infrastructure.IoC.Modules;
using JwtAuth.Infrastructure.Repositories;

namespace JwtAuth.Infrastructure.IoC
{
	public class ContainerModule : Autofac.Module
	{
		private readonly IConfiguration _configuration;

		public ContainerModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(AutoMapperConfig.Initialize())
				.SingleInstance();

			builder.RegisterModule<RepositoryModule>();
			builder.RegisterModule<ServiceModule>();
			builder.RegisterModule(new SettingsModule(_configuration));
		}
	}
}
