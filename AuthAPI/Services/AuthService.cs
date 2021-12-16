using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MoneyTrackDatabaseAPI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MoneyTrackDatabaseAPI.Services
{
    public class AuthService : IAuthService
    {
        private string secretAccess;
        private string secretRefresh;
        public AuthModel AuthModel { get; set; }
        public bool IsTokenValid { get; set; }
        private ITokenService tokenService;
        private int refreshTokenTTL;
        private int accessTokenTTL;

        public AuthService( ITokenService tokenService)
        {
            secretAccess = Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET");
            secretRefresh = Environment.GetEnvironmentVariable("REFRESH_TOKEN_SECRET");
            refreshTokenTTL = int.Parse(Environment.GetEnvironmentVariable("REFRESH_TOKEN_TTL"));
            accessTokenTTL = int.Parse(Environment.GetEnvironmentVariable("ACCESS_TOKEN_TTL"));
            this.tokenService = tokenService;

            AuthModel = null;
            IsTokenValid = false;
        }

        public async Task<string> GenerateAccessToken(string refreshToken)
        {
            if (!await tokenService.ContainsToken(refreshToken))
                return null;

            var payload = await GetPayloadRefresh(refreshToken);
            var newPayload = new AuthModel(payload.UserId, accessTokenTTL);
            
            IJwtAlgorithm algorithm = new HMACSHA512Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(newPayload, secretAccess);
            return token;
        }

        public async Task<string> GenerateRefreshToken(int userId)
        {
         
            var model = new AuthModel(userId, refreshTokenTTL);
            IJwtAlgorithm algorithm = new HMACSHA512Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(model, secretRefresh);
            return token;
        }
        public async Task<string> GenerateAccessToken(int userId)
        {
         
            var model = new AuthModel(userId, accessTokenTTL);
            IJwtAlgorithm algorithm = new HMACSHA512Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(model, secretAccess);
            return token;
        }

        public async Task<AuthModel> GetPayloadAccess(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA512Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
    
                var json = decoder.Decode(token, secretAccess, true);
                var ret = JsonSerializer.Deserialize<AuthModel>(json);
                return ret;
        }
        public async Task<AuthModel> GetPayloadRefresh(string token )
        {
            IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA512Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
    
                var json = decoder.Decode(token, secretRefresh, true);
                var ret = JsonSerializer.Deserialize<AuthModel>(json);
                return ret;
        }
        public bool IsValidToken(string token)
        {
            return false;
        }
    }
}