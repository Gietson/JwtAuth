using JwtAuth.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.Api.Controllers
{
	[Route("[controller]")]
	public class AuthController : Controller
	{
		private readonly IJwtHandler _jwtHandler;
		private readonly IUserService _userService;

		public AuthController(IJwtHandler jwtHandler, IUserService userService)
		{
			_jwtHandler = jwtHandler;
			_userService = userService;
		}

		[HttpGet]
		[Route("token")]
		public async Task<IActionResult> Get()
		{
			var user = await _userService.GetAsync("user1@test.com");
			var token = _jwtHandler.CreateToken(user.Id, user.Role);

			return Json(token);
		}

		[HttpGet]
		[Authorize]
		[Route("auth")]
		public IActionResult GetAuth()
		{
			return Content("Auth");
		}
	}
}
