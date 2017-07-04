using AutoMapper;
using JwtAuth.Infrastructure.Domain;
using JwtAuth.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.Infrastructure.Mappers
{
	public class AutoMapperConfig
	{
		public static IMapper Initialize()
			=> new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<User, UserDto>().ReverseMap();
			})
			.CreateMapper();
	}
}
