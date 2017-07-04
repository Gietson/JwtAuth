using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;
using JwtAuth.Infrastructure.Services;

namespace JwtAuth.Infrastructure.IoC.Modules
{
	public class ServiceModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var assembly = typeof(ServiceModule)
				.GetTypeInfo()
				.Assembly;

			builder.RegisterAssemblyTypes(assembly)
				   .Where(x => x.IsAssignableTo<IService>())
				   .AsImplementedInterfaces()
				   .InstancePerLifetimeScope();

			builder.RegisterType<Encrypter>()
				   .As<IEncrypter>()
				   .SingleInstance();

			builder.RegisterType<JwtHandler>()
				   .As<IJwtHandler>()
				   .SingleInstance();
		}
	}
}
