using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class AccessListRepository : BaseRepository
	{
		public AccessListRepository(AppDbContext dbContext) : base(dbContext) {}

		public IQueryable<AccessListEntity> GetReadOnlyQuery() => _dbContext.AccessLists.AsNoTracking();

		/// <summary>
		/// Gets accessList along with locations, and users. Could be an expensive call.
		/// </summary>
		/// <param name="accessListId"></param>
		/// <returns></returns>
		public async Task<AccessListEntity> GetFullAccessList(int accessListId)
		{
			return await GetReadOnlyQuery()
							.Include(oo => oo.Users).ThenInclude(oo => oo.User)
							.Include(oo => oo.Locations).ThenInclude(oo => oo.Location)
							.SingleAsync(oo => oo.Id == accessListId);
		}
	}
}
