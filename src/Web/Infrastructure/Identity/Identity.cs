using System;

namespace Phobos.Web.Infrastructure.Identity
{
	public class Identity
	{
		public int ID { get; set; }

		public Guid UID { get; set; }

		public string Name { get; set; }
	}

	public static class IdentityConstants
	{
		public const string IdentityClaimKey = "Phobos_Claim_Identity";

		public const string IdentityRequestKey = "Phobos_Web_Request_Identity";
	}
}