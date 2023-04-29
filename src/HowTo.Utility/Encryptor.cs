using System.Security.Cryptography;

namespace HowTo.Utility
{
    public class Encryptor
    {
        private const int Iterations = 10000;

        public string HashEncrypt(string plainTextValue, byte[] salt)
        {
            //byte[] salt = RandomNumberGenerator.GetBytes(16);

            var pbkdf2 = new Rfc2898DeriveBytes(plainTextValue, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
