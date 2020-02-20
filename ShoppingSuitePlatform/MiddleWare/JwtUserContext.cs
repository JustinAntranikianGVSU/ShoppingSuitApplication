
namespace ShoppingSuitePlatform.MiddleWare
{
	public class JwtUserContext
	{
		public int LoggedInUserId { get; set; }

		public int ImpersonationUserId { get; set; }
	}
}
