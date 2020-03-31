using CoreLibrary.ServiceResults;
using DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace CoreLibrary.Orchestrators
{
	public abstract class OrchestratorBase<T> : OrchestratorBase where T : class
	{
		protected OrchestratorBase() {}

		protected OrchestratorBase(AppDbContext dbContext) : base(dbContext) {}

		protected OrchestratorBase(JwtRequestContext jwtRequestContext) : base(jwtRequestContext) {}

		protected OrchestratorBase(AppDbContext dbContext, JwtRequestContext jwtRequestContext) : base(dbContext, jwtRequestContext) {}

		protected ServiceResult<T> GetProcessedResult(T value) => base.GetProcessedResult(value);

		protected ServiceResult<T> GetBadRequestResult(params ServiceError[] errors) => GetBadRequestResult<T>(errors);

		protected ServiceResult<T> GetBadRequestResult(List<ServiceError> errors) => new ServiceResult<T>(errors, ServiceResultStatus.BadRequest);

		protected ServiceResult<T> GetBadRequestResult(string errorMessage) => GetBadRequestResult<T>(errorMessage);
	}

	public abstract class OrchestratorBase : ServiceErrorResultBase
	{
		protected readonly AppDbContext _dbContext;
		protected readonly JwtRequestContext _jwtRequestContext;

		protected OrchestratorBase() {}

		protected OrchestratorBase(AppDbContext dbContext) => _dbContext = dbContext;

		protected OrchestratorBase(JwtRequestContext jwtRequestContext) => _jwtRequestContext = jwtRequestContext;

		protected OrchestratorBase(AppDbContext dbContext, JwtRequestContext jwtRequestContext)
		{
			(_dbContext, _jwtRequestContext) = (dbContext, jwtRequestContext);
		}

		protected ServiceResult<T> GetProcessedResult<T>(T value) where T : class
		{
			return new ServiceResult<T>(value, ServiceResultStatus.Processed);
		}

		protected ServiceResult<T> GetBadRequestResult<T>(params ServiceError[] errors) where T : class
		{
			return new ServiceResult<T>(errors.ToList(), ServiceResultStatus.BadRequest);
		}

		protected ServiceResult<T> GetBadRequestResult<T>(string errorMessage) where T : class
		{
			var errors = new[] { GetError(errorMessage) };
			return new ServiceResult<T>(errors.ToList(), ServiceResultStatus.BadRequest);
		}
	}
}
