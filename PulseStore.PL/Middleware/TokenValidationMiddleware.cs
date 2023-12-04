using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace PulseStore.PL.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;

    public TokenValidationMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var hasAuthorize = context.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>() is not null;

        if(hasAuthorize)
        {
            string accessToken = context.Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim();

            var identityServerHost = configuration["IdentityServer:Host"];

            if (!accessToken.IsNullOrEmpty() && identityServerHost is not null)
            {
                string blacklistUrl = $"{identityServerHost}/connect/token/check?accessToken={accessToken}";

                var httpClient = _httpClientFactory.CreateClient();

                try
                {
                    var response = await httpClient.GetAsync(blacklistUrl);
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        context.Response.StatusCode = 499; // custom status code to indicate that access_token is blacklisted
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync("access_token is in the blacklist");
                        return;
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "text/plain";
                        string responseContent = await response.Content.ReadAsStringAsync();
                        await context.Response.WriteAsync($"blacklist error check: {responseContent}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(
                        $"an exception occured during the request to the Duende Identity Server: {ex.Message}");
                    return;
                }
            }
        }

        await _next(context);
    }
}