using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;

namespace Phobos.Api.Infrastructure.Providers
{
	public class HttpCorsPolicyProvider : System.Web.Http.Cors.ICorsPolicyProvider
	{
		private readonly Lazy<CorsPolicy> corsPolicyCache;

		public HttpCorsPolicyProvider()
		{
			corsPolicyCache = new Lazy<CorsPolicy>(() => CreateCorsPolicy(), true);
		}

		public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task.FromResult(corsPolicyCache.Value);
		}

		private CorsPolicy CreateCorsPolicy()
		{
			CorsPolicy corsPolicy = new CorsPolicy();
			corsPolicy.AllowAnyHeader = true;
			corsPolicy.AllowAnyMethod = true;

			corsPolicy.Origins.Add("http://localhost:63332");

			return corsPolicy;
		}
	}
}