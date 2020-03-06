using DataAccess;

namespace CoreLibrary.Orchestrators
{
	public abstract class JwtContextOrchestratorBase<T> : DbContextOrchestratorBase<T> where T : class
	{
		protected readonly JwtRequestContext _jwtRequestContext;

		protected JwtContextOrchestratorBase(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext) => _jwtRequestContext = jwtRequestContext;
	}
}
