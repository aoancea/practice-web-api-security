using System.Threading.Tasks;
using System.Web.Http;

namespace Phobos.Api.Controllers
{
	[Authorize]
	public class ValuesController : ApiController
	{
		private readonly Infrastructure.Identity.IIdentityProvider identityProvider;

		public ValuesController(Infrastructure.Identity.IIdentityProvider identityProvider)
		{
			this.identityProvider = identityProvider;
		}

		[ActionName("Get")]
		public async Task<string[]> GetAsync()
		{
			Infrastructure.Identity.Identity identity = identityProvider.Identity(this);

			return await Task.FromResult(new string[5] { "string1", "string2", "string3", "string4", "string5" });
		}
	}
}