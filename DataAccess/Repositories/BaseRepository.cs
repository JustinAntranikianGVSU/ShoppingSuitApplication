namespace DataAccess.Repositories
{
	public abstract class BaseRepository
	{
		protected readonly AppDbContext _dbContext;

		protected BaseRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
