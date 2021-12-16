using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public interface IAuthService
    {
        public AuthModel AuthModel { get; set; }
        public bool IsTokenValid { get; set; }
        Task<string> GenerateAccessToken(string refreshToken);
        Task<string> GenerateAccessToken(int userId);
        Task<string> GenerateRefreshToken(int userId);
        Task<AuthModel> GetPayloadAccess(string token);
        Task<AuthModel> GetPayloadRefresh(string token);
    }
}