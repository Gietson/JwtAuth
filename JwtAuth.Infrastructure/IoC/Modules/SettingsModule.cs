using Autofac;
using JwtAuth.Infrastructure.Extensions;
using JwtAuth.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;

namespace JwtAuth.Infrastructure.IoC.Modules
{
	public class SettingsModule : Autofac.Module
	{
		private readonly IConfiguration _configuration;

		public SettingsModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(_configuration.GetSettings<GeneralSettings>())
				   .SingleInstance();
			builder.RegisterInstance(_configuration.GetSettings<JwtSettings>())
				   .SingleInstance();
		}
	}
}
