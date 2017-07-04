using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.Infrastructure.Extensions
{
	public static class StringExtensions
	{
		public static bool Empty(this string value)
			=> string.IsNullOrWhiteSpace(value);
	}
}
