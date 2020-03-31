using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace CoreLibrary
{
	public class ClaimsManager
	{
		private readonly int _id;
		private readonly Guid? _clientIdentifier;
		private readonly IEnumerable<Guid> _roleIdentifiers;

		public ClaimsManager(int id, Guid? clientIdentifier, IEnumerable<Guid> roleIdentifiers)
		{
			_id = id;
			_clientIdentifier = clientIdentifier;
			_roleIdentifiers = roleIdentifiers;
		}

		/// <summary>
		/// Generates the Sub, ClientId, and Role claims for the user.
		/// </summary>
		/// <returns></returns>
		public List<Claim> GetClaims()
		{
			var userClaims = new[]
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
			var userClaims = new[]
			{
				new Claim(AppClaimTypes.ImpersonationUserId, GetUserIdForClaims()),
				new Claim(AppClaimTypes.ImpersonationClientId, GetClientIdForClaims())
			};

			return MergeWithRoleClaims(userClaims);
		}

		private string GetUserIdForClaims() => _id.ToString();

		/// <summary>
		/// return ClientIdentifier or empty guid as string. The JwtRequestContextHelper class will check for an empty guid so don't use null.
		/// </summary>
		/// <returns></returns>
		private string GetClientIdForClaims() => (_clientIdentifier ?? Guid.Empty).ToString();

		private List<Claim> MergeWithRoleClaims(IEnumerable<Claim> userClaims) => GetRoleClaims().Concat(userClaims).ToList();

		private IEnumerable<Claim> GetRoleClaims()
		{
			return _roleIdentifiers.Select(oo => new Claim(ClaimTypes.Role, oo.ToString()));
		}
	}
}
