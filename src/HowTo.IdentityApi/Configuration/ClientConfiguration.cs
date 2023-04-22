using IdentityServer4.Models;

namespace HowTo.IdentityApi.Configuration
{
    public static class ClientConfiguration
    {
        public static IEnumerable<Client> Clients => new List<Client>
        {
            ClientCreator.CreateForClientCredentials("webapp", "b20d7f1c-3d10-4c72-b994-8bd36a847e28", Scope.WebApp),
            ClientCreator.CreateForPassword("demo", "11f0c7b1-d73d-4c6f-8409-1b3398a2e541", Scope.ReadOnly),
            ClientCreator.CreateForPassword("test", "ee09b556-a7ab-40cf-9a14-cd9e629a6bb7", Scope.WebApp)
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
