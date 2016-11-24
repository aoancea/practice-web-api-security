using System.Web;

namespace Phobos.Web.Infrastructure.Identity
{
	public static class IdentityExtensions
	{
		public static void AddIdentity(this HttpContextBase httpContext, Identity identity)
		{
			httpContext.Items.Add(IdentityConstants.IdentityRequestKey, identity);
		}

		public static Identity GetIdentity(this HttpContextBase httpContext)
		{
			return httpContext.Items[IdentityConstants.IdentityRequestKey] as Identity;
		}
	}
}