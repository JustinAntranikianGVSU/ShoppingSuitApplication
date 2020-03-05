using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class AccessListRepository : BaseRepository
	{
		public AccessListRepository(AppDbContext dbContext) : base(dbContext) {}

		/// <summary>
		/// Gets accessList along with locations, and users. Could be an expensive call.
		/// </summary>
		/// <param name="accessListId"></param>
		/// <returns></returns>
		public async Task<AccessListEntity> GetFullAccessList(int accessListId)
		{
			return await _dbContext.AccessLists
							.Include(oo => oo.Users).ThenInclude(oo => oo.User)
							.Include(oo => oo.Locations).ThenInclude(oo => oo.Location)
							.AsNoTracking()
							.SingleAsync(oo => oo.Id == accessListId);
		}
	}
}
