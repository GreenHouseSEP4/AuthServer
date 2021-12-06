using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public interface ITokenService
    {
        Task AddToken(string token);
        Task<bool> ContainsToken(string token);
        Task Logout(string token);
    }
}