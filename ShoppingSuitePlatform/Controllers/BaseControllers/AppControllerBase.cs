using CoreLibrary.ServiceResults;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers.BaseControllers
{
	public abstract class AppControllerBase : ControllerBase
	{
		protected ActionResult BadRequestIfNotProcessed<T>(ServiceResult<T> result) where T : class
		{
			if (result.NotProcessed)
			{
				return BadRequest(result.Errors);
			}

			return Ok(result.Value);
		}

		protected ActionResult NotFoundIfNotProcessed<T>(ServiceResult<T> result) where T : class
		{
			if (result.NotProcessed)
			{
				return NotFound(result.Errors);
			}

			return Ok(result.Value);
		}
	}
}
