using JwtAuth.Infrastructure.DTO;
using System;

namespace JwtAuth.Infrastructure.Services
{
	public interface IJwtHandler
	{
		JwtDto CreateToken(Guid userId, string role);
	}
}
