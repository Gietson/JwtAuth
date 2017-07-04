using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuth.Infrastructure.DTO;
using JwtAuth.Infrastructure.Settings;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using JwtAuth.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Infrastructure.Services
{
	public class JwtHandler : IJwtHandler
	{
		private readonly JwtSettings _settings;

		public JwtHandler(JwtSettings settings)
		{
			_settings = settings;
		}

		public JwtDto CreateToken(Guid userId, string role)
		{
			var now = DateTime.UtcNow;
			var claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
				new Claim(ClaimTypes.Role, role),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString(), ClaimValueTypes.Integer64)
			};

			var expires = now.AddMinutes(_settings.ExpiryMinutes);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
			var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var jwt = new JwtSecurityToken(
					issuer: _settings.Issuer,
					claims: claims,
					notBefore: now,
					expires: expires,
					signingCredentials: signingCredentials
				);
			var token = new JwtSecurityTokenHandler().WriteToken(jwt);

			return new JwtDto
			{
				Token = token,
				Expires = jwt.ValidTo
			};
		}
	}
}
