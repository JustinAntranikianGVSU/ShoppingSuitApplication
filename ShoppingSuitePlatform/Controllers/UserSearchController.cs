using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary;
using Domain.Orchestrators.Users;
using ShoppingSuitePlatform.Controllers.BaseControllers;
using Domain.Dtos;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserSearchController : AppControllerBase
	{
		private readonly IUserSearchOrchestrator _userSearchOrchestrator;

		public UserSearchController(IUserSearchOrchestrator userSearchOrchestrator)
		{
			_userSearchOrchestrator = userSearchOrchestrator;
		}

		[HttpPost]
		[Authorize(Policy = AppPolicy.ViewEmployee)]
		public async Task<ActionResult> Post([FromBody] UserSearchRequestViewModel viewModel)
		{
			var result = await _userSearchOrchestrator.GetSearchResults(viewModel);
			return BadRequestIfNotProcessed(result);
		}
	}
}
