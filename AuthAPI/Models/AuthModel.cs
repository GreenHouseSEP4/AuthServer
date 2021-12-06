using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MoneyTrackDatabaseAPI.Models
{
    public class AuthModel
    {
        public AuthModel(int userId,int ttl)
        {
            UserId = userId;
            exp = DateTimeOffset.UtcNow.AddSeconds(ttl).ToUnixTimeSeconds();
        }

        public AuthModel()
        {
        }
        public long exp { get; set; }
        public int UserId { get; set; }
    }
}