using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingSuitePlatform.MiddleWare
{
	public class AppUserStore : IUserStore<IdentityUser>
	{
		public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public void Dispose() { }

		public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			return Task.FromResult(new IdentityUser("My User Dude"));
		}

		public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult("My User Dude");

		}

		public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult("My User Dude");
		}

		public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult("My User Dude");

		}

		public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
