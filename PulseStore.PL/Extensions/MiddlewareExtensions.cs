using PulseStore.PL.Middleware;

namespace PulseStore.PL.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}