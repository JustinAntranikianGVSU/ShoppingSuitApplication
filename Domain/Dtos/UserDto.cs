using CoreLibrary;
using CoreLibrary.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Domain
{
	public class UserDto : UserBasicDto
	{
		public string Email { get; set; }

		public List<Role> Roles { get; set; } = new List<Role>();

		public Guid? ClientIdentifier { get; set; }

		public UserDto(int id, string firstName, string lastName) : base(id, firstName, lastName) {}

		/// <summary>
		/// Generates the Sub, ClientId, and Role claims for the user.
		/// </summary>
		/// <returns></returns>
		public List<Claim> GetUserClaims()
		{
			var roleClaims = Roles.Select(oo => new Claim(ClaimTypes.Role, oo.Identifier.ToString()));
			var clientId = ClientIdentifier ?? Guid.Empty;

			var userClaims = new []
			{
				new Claim(JwtRegisteredClaimNames.Sub, Id.ToString()),
				new Claim(AppClaimTypes.ClientId, clientId.ToString())
			};

			return roleClaims.Concat(userClaims).ToList();
		}
	}
}
