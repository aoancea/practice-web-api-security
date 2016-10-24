using System;

namespace Phobos.Api.Infrastructure.Helpers
{
	public interface IAccessTokenHelper
	{
		TimeSpan AccessTokenExpireTimeSpan();
	}

	public class AccessTokenHelper : IAccessTokenHelper
	{
		public TimeSpan AccessTokenExpireTimeSpan()
		{
			return TimeSpan.FromMinutes(30);
		}
	}
}