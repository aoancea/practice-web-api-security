using System.Web;

namespace Phobos.Web.Infrastructure.Identity
{
	public interface IIdentityProvider
	{
		Identity Identity(HttpContextBase httpContext);
	}

	public class IdentityProvider : IIdentityProvider
	{
		public Identity Identity(HttpContextBase httpContext)
		{
			return httpContext.Items[IdentityConstants.IdentityRequestKey] as Identity;
		}
	}
}