using Phobos.Api.Infrastructure.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace Phobos.Api.Controllers
{
	[Authorize]
	public class ValuesController : ApiController
	{
		[ActionName("Get")]
		public async Task<string[]> GetAsync()
		{
			Identity identity = Request.GetIdentity();

			return await Task.FromResult(new string[5] { "string1", "string2", "string3", "string4", "string5" });
		}
	}
}