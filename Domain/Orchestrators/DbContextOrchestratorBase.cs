using DataAccess;

namespace Domain.Orchestrators
{
	public abstract class DbContextOrchestratorBase<T> : OrchestratorBase<T> where T : class
	{
		protected readonly AppDbContext _dbContext;

		protected DbContextOrchestratorBase(AppDbContext dbContext) => _dbContext = dbContext;
	}
}
