using System.Collections.Generic;
using System.Security.Claims;

namespace Domain.Dtos
{
	public class LoginRequestDto
	{
		public string Email { get; set; }

		public string PassWord { get; set; }
	}

	public class LoginReponseDto
	{
		public List<Claim> Claims { get; set; }

		public int UserId { get; set; }

		public LoginReponseDto(List<Claim> claims, int userId)
		{
			Claims = claims;
			UserId = userId;
		}
	}
}
