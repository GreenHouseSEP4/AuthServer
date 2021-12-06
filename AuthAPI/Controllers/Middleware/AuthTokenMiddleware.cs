using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Controllers.Middleware
{
    public class AuthTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            var request = context.Request;
            
            try
            {
                string token = request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    // Call the next delegate/middleware in the pipeline
                    await _next(context);
                }
                else
                {
                    token = token.Split(" ")[1];
                    var authModel = await authService.GetPayloadAccess(token);
                    authService.AuthModel = authModel;
                    await _next(context);
                }
            }
            catch (Exception e)
            {
                authService.AuthModel = null;
                context.Response.StatusCode = 403;
                var response = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new ApiError(e.Message)));
                await context.Response.Body.WriteAsync(response,0,response.Length) ;
                return;
            }
        }
        
    }
}