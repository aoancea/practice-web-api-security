using System;

namespace Phobos.Api.Infrastructure.Identity
{
	public class Identity
	{
		public int ID { get; set; }

		public Guid UID { get; set; }

		public string Name { get; set; }
	}

	public static class IdentityConstants
	{
		public const string IdentityClaimKey = "Phobos_Api_Claim_Identity";

		public const string IdentityRequestKey = "Phobos_Api_Request_Identity";
	}
}