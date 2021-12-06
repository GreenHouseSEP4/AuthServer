using Microsoft.AspNetCore.Builder;

namespace MoneyTrackDatabaseAPI.Controllers.Middleware
{

        public static class AuthTokenMiddlewareException
        {
            public static IApplicationBuilder UseAuthTokenMiddleware(
                this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<AuthTokenMiddleware>();
            }
        }
}