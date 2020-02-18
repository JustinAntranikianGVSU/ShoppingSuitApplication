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

		public LoginReponseDto(List<Claim> claims) => Claims = claims;
	}
}
