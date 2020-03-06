using CoreLibrary;
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
		public List<Claim> GetClaims()
		{
			var userClaims = new []
			{
				new Claim(JwtRegisteredClaimNames.Sub, GetUserIdForClaims()),
				new Claim(AppClaimTypes.ClientId, GetClientIdForClaims())
			};

			return MergeWithRoleClaims(userClaims);
		}

		/// <summary>
		/// Generates ImpersonationUserId, ImpersonationClientId, and Role claims when trying to impersonate a user.
		/// </summary>
		/// <returns></returns>
		public List<Claim> GetClaimsForImpersonation()
		{
			var userClaims = new []
			{
				new Claim(AppClaimTypes.ImpersonationUserId, GetUserIdForClaims()),
				new Claim(AppClaimTypes.ImpersonationClientId, GetClientIdForClaims())
			};

			return MergeWithRoleClaims(userClaims);
		}

		private string GetUserIdForClaims() => Id.ToString();

		/// <summary>
		/// return ClientIdentifier or empty guid as string. The JwtRequestContextHelper class will check for an empty guid so don't use null.
		/// </summary>
		/// <returns></returns>
		private string GetClientIdForClaims() => (ClientIdentifier ?? Guid.Empty).ToString();

		private List<Claim> MergeWithRoleClaims(IEnumerable<Claim> userClaims) => GetRoleClaims().Concat(userClaims).ToList();

		private IEnumerable<Claim> GetRoleClaims()
		{
			return Roles.Select(oo => new Claim(ClaimTypes.Role, oo.Identifier.ToString()));
		}
	}
}
