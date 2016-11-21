using System.Web.Http;

namespace Phobos.Api.Infrastructure.Identity
{
	public interface IIdentityProvider
	{
		Identity Identity(ApiController controller);
	}

	public class IdentityProvider : IIdentityProvider
	{
		public Identity Identity(ApiController controller)
		{
			return controller.Request.Properties[IdentityConstants.IdentityRequestKey] as Identity;
		}
	}
}