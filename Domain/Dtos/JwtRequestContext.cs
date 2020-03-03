using System;

namespace Domain.Dtos
{
	public class JwtRequestContext
	{
		public int LoggedInUserId { get; set; }

		public Guid? ClientIdentifier { get; set; }

		public int? ImpersonationUserId { get; set; }

		public Guid? ImpersonationClientIdentifier { get; set; }

		/// <summary>
		/// If ImpersonationUserId is set return that. Otherwise return the LoggedInUserId.
		/// </summary>
		/// <returns></returns>
		public int GetUserId() => ImpersonationUserId ?? LoggedInUserId;

		/// <summary>
		/// If ImpersonationClientIdentifier is set return that. Otherwise return the ClientIdentifier.
		/// </summary>
		/// <returns></returns>
		public Guid? GetClientId() => ImpersonationClientIdentifier ?? ClientIdentifier;
	}
}
