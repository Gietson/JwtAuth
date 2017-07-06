using JwtAuth.Api.Filters;
using JwtAuth.Api.Models;
using JwtAuth.Infrastructure.DTO;
using JwtAuth.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JwtAuth.Api.Controllers
{
	[ValidateModel]
	[Route("api/[controller]")]
	public class AuthController : Controller
	{
		private readonly IJwtHandler _jwtHandler;
		private readonly IUserService _userService;

		public AuthController(IJwtHandler jwtHandler, IUserService userService)
		{
			_jwtHandler = jwtHandler;
			_userService = userService;
		}

		[HttpPost]
		[Route("local")]
		public async Task<IActionResult> Post([FromBody] LoginViewModel model)
		{
			await _userService.LoginAsync(model.Email, model.Password);
			UserDto user = await _userService.GetAsync(model.Email);
			JwtDto token = _jwtHandler.CreateToken(user.Id, user.Role);

			return Ok(new { token = token.Token, user = user });
		}

	}
}
