using Microsoft.AspNet.Identity.EntityFramework;

namespace Phobos.Api.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
            : base("applicationDbContext")
        { }
    }
}