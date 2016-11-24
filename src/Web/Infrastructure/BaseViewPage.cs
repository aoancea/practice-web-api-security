using Phobos.Web.Infrastructure.Identity;
using System;

namespace Phobos.Web.Infrastructure
{
	public abstract class BaseViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
		private readonly Lazy<Identity.Identity> identityCache;

		public BaseViewPage()
		{
			identityCache = new Lazy<Identity.Identity>(() => Context.GetIdentity());
		}

		public Identity.Identity Identity { get { return identityCache.Value; } }
	}
}