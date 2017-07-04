using System.Threading.Tasks;

namespace JwtAuth.Infrastructure.Services
{
	public interface IDataInitializer : IService
	{
		Task SeedAsync();
	}
}