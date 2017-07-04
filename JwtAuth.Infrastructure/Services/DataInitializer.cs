using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Infrastructure.Services
{
	public class DataInitializer : IDataInitializer
	{
		private readonly IUserService _userService;
		private readonly ILogger<DataInitializer> _logger;

		public DataInitializer(IUserService userService, ILogger<DataInitializer> logger)
		{
			_userService = userService;
			_logger = logger;
		}

		public async Task SeedAsync()
		{
			var users = await _userService.BrowseAsync();
			if (users.Any())
			{
				_logger.LogDebug("Data was already initialized.");

				return;
			}
			_logger.LogDebug("Initializing data...");
			var tasks = new List<Task>();
			for (var i = 1; i <= 10; i++)
			{
				var userId = Guid.NewGuid();
				var username = $"user{i}";
				await _userService.RegisterAsync(userId, $"user{i}@test.com",
												 username, "secret", "user");
				_logger.LogDebug($"Adding user: '{username}'.");
			}
			for (var i = 1; i <= 3; i++)
			{
				var userId = Guid.NewGuid();
				var username = $"admin{i}";
				_logger.LogDebug($"Adding admin: '{username}'.");
				tasks.Add(_userService.RegisterAsync(userId, $"admin{i}@test.com",
					username, "secret", "admin"));
			}
			await Task.WhenAll(tasks);
			_logger.LogDebug("Data was initialized.");
		}
	}
}
