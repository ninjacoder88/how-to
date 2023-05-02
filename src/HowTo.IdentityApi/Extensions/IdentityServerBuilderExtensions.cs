using System.Security.Cryptography.X509Certificates;

namespace HowTo.IdentityApi.Extensions
{
    public static class IdentityServerBuilderExtensions
    {
        public static void AddProductionCredential(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            if(TryFindCertificate("CertificateThumbprintPrimary", out X509Certificate2? primaryCertificate))
            {
                builder.AddSigningCredential(primaryCertificate);
                return;
            }

            if (TryFindCertificate("CertificateThumbprintSecondary", out X509Certificate2? secondaryCertificate))
            {
                builder.AddSigningCredential(secondaryCertificate);
                return;
            }

            throw new Exception("Could not find a vali certificate matching the provided thumbprint");
        }

        private static bool TryFindCertificate(string thumbprint, out X509Certificate2? certificate)
        {
            certificate = null;

            if(string.IsNullOrEmpty(thumbprint))
                return false;

            using(var store = new X509Store(StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);

                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if(certs.Count == 0)
                    return false;

                var cert = certs.OfType<X509Certificate2>().SingleOrDefault();

                if(cert == null)
                    return false;

                if(cert.NotAfter > DateTime.Now)
                {
                    certificate = cert;
                    return true;
                }
            }

            return false;
        }
    }
}
