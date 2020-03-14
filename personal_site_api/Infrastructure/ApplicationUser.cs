using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace personal_site_api.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        //no additions to IdentityUser Model

        //called in CustomOAuthProvider to generate ClaimsIdentity from ApplicationUser
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}