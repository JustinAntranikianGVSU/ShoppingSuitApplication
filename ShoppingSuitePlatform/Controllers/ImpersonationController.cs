using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingSuitePlatform.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImpersonationController : ControllerBase
    {
		public void Post([FromBody] int impersonatingUserId)
		{
			HttpContext.Session.SetString("impersonatingUserId", impersonatingUserId.ToString());
		}
    }
}