using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DedicadoEstudo.Service.CRiptografia
{
    public class PasswordHasher
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        public static string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm))
                {
                    var hash = rfc2898.GetBytes(KeySize);
                    return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
                }
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                throw new FormatException("The hashed password is not in the correct format.");
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm))
            {
                var computedHash = rfc2898.GetBytes(KeySize);
                return CryptographicOperations.FixedTimeEquals(computedHash, hash);
            }
        }
    }
}
