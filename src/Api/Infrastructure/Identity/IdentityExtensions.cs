using System.Net.Http;

namespace Phobos.Api.Infrastructure.Identity
{
	public static class IdentityExtensions
	{
		public static void AddIdentity(this HttpRequestMessage httpRequestMessage, Identity identity)
		{
			httpRequestMessage.Properties.Add(IdentityConstants.IdentityRequestKey, identity);
		}

		public static Identity GetIdentity(this HttpRequestMessage httpRequestMessage)
		{
			return httpRequestMessage.Properties[IdentityConstants.IdentityRequestKey] as Identity;
		}
	}
}