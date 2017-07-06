using JwtAuth.Api.Filters;
using JwtAuth.Api.Models;
using JwtAuth.Infrastructure.DTO;
using JwtAuth.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JwtAuth.Api.Controllers
{
	[Route("api/[controller]")]
	[ValidateModel]
	public class UserController : Controller
	{
		private readonly IJwtHandler _jwtHandler;
		private readonly IUserService _userService;

		public UserController(IJwtHandler jwtHandler, IUserService userService)
		{
			_jwtHandler = jwtHandler;
			_userService = userService;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
		{
			try
			{
				string role = "user";
				await _userService.RegisterAsync(Guid.NewGuid(), model.Email, model.Username, model.Password, role);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest($"Couldn't register new user, ex: {ex}");
			}
		}

		[HttpGet]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<UserDto> users = await _userService.BrowseAsync();
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest($"Couldn't get users, ex: {ex}");
			}

		}
	}
}
