using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public class SecurityService
    {
        public User HashCredentials(User user)
        {
            var userSalt = CreateSalt();
            user.Salt = userSalt;
            user.Password = Convert.ToBase64String(HashPasswordV1(user.Password,userSalt));
            user.HashVersion = Environment.GetEnvironmentVariable("PASSWORD_HASH_VERSION");
            user.ApiVersion = Environment.GetEnvironmentVariable("API_VERSION");
            user.RegistrationDate = DateTime.Now;
            return user;
        }
        private string CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        private byte[] HashPasswordV1(string password, string salt)
        {
            var decodedSalt = Convert.FromBase64String(salt);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = decodedSalt;
            argon2.DegreeOfParallelism = 1; // one core
            argon2.Iterations = 1;
            argon2.MemorySize = 128*128; // 128 MB

            return argon2.GetBytes(16);
        }
        private byte[] HashPasswordV2(string password, string salt)
        {
            var decodedSalt = Convert.FromBase64String(salt);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = decodedSalt;
            argon2.DegreeOfParallelism = 1; // one core
            argon2.Iterations = 2;
            argon2.MemorySize = 256*256; // 256 MB

            return argon2.GetBytes(16);
        }
        
        public bool VerifyHash(string password, string salt, string hash)
        {
            var decodedHash = Convert.FromBase64String(hash);
            var newHash = HashPasswordV1(password, salt);
            return decodedHash.SequenceEqual(newHash);
        }
    }
}