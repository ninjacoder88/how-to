using System.Security.Cryptography.X509Certificates;

namespace HowTo.IdentityApi.Extensions
{
    public static class IdentityServerBuilderExtensions
    {
        public static void AddProductionCredential(this IIdentityServerBuilder builder, IConfiguration configuration)
        {

        }

        private static bool TryFindCertificate(string thumbprint, out X509Certificate2 certificate)
        {
            certificate = null;
            return true;
        }
    }
}
