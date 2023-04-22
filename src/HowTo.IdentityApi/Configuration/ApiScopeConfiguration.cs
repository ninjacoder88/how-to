using IdentityServer4.Models;

namespace HowTo.IdentityApi.Configuration
{
    public static class ApiScopeConfiguration
    {
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope(Scope.WebApp, "Web App"),
            new ApiScope(Scope.ReadOnly, "Read Only")
        };
    }
}
