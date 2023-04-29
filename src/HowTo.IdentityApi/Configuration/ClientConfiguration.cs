using IdentityServer4.Models;

namespace HowTo.IdentityApi.Configuration
{
    public static class ClientConfiguration
    {
        public static IEnumerable<Client> Clients => new List<Client>
        {
            ClientCreator.CreateForClientCredentials("swagger", "b20d7f1c-3d10-4c72-b994-8bd36a847e28", Scope.WebApp),
            ClientCreator.CreateForPassword("webapplogin", "7c6e005b-aaf7-425a-a8c4-f488ff5dd95b", Scope.WebApp)
        };
    }

    internal static class ClientCreator
    {
        public static Client CreateForClientCredentials(string clientId, string secret, params string[] scopes)
        {
            return new Client
            {
                ClientId = clientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret(secret.Sha256()) },
                AllowedScopes = scopes
            };
        }

        public static Client CreateForPassword(string clientId, string secret, params string[] scopes)
        {
            return new Client
            {
                ClientId = clientId,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret(secret.Sha256()) },
                AllowedScopes = scopes
            };
        }
    }
}
