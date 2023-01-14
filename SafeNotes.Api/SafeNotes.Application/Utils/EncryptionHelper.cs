using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace SafeNotes.Application.Utils
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string message, byte[] hashedKey)
        {
            using var aes = new AesGcm(hashedKey);

            var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            RandomNumberGenerator.Fill(nonce);

            var plaintextBytes = Encoding.UTF8.GetBytes(message);
            var ciphertext = new byte[plaintextBytes.Length];
            var tag = new byte[AesGcm.TagByteSizes.MaxSize];

            aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

            return $"{Convert.ToBase64String(ciphertext)}.{Convert.ToBase64String(nonce)}.{Convert.ToBase64String(tag)}";
        }

        public static string Decrypt(string cipherText, byte[] hashedKey)
        {
            using var aes = new AesGcm(hashedKey);

            var split = cipherText.Split('.');
            var cipherBytes = Convert.FromBase64String(split[0]);
            var nonce = Convert.FromBase64String(split[1]);
            var tag = Convert.FromBase64String(split[2]);

            var plaintextBytes = new byte[cipherBytes.Length];
            aes.Decrypt(nonce, cipherBytes, tag, plaintextBytes);

            return Encoding.UTF8.GetString(plaintextBytes);
        }

        public static (byte[], string) StretchKey(string key)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            var hash = KeyDerivation.Pbkdf2(
                password: key,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return (hash, Convert.ToBase64String(salt));
        }

        public static byte[] HashKey(string key, string saltString)
        {
            var salt = Convert.FromBase64String(saltString);

            var hashed = KeyDerivation.Pbkdf2(
                password: key,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return hashed;
        }
    }
}
