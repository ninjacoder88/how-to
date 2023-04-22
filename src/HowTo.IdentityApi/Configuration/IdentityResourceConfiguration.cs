using IdentityServer4.Models;

namespace HowTo.IdentityApi.Configuration
{
    public static class IdentityResourceConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[] { new IdentityResources.OpenId() };
    }
}
