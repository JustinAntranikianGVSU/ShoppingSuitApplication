using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShoppingSuitePlatform.Controllers
{
	[ApiController]
	public class UserInfoController : ControllerBase
	{
		[HttpGet]
		public async Task<UserBasicInfo> Get()
		{
			return new UserBasicInfo();
		}
	}
}
