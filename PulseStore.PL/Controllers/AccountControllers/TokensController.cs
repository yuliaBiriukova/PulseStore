using System.Text.Json;
using IdentityModel.Client;
using PulseStore.PL.Helpers;
using PulseStore.PL.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AccountControllers
{
    [Route("api/account/[controller]")]
    [ApiExplorerSettings(GroupName = "Account/Tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public TokensController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        ///     Obtains new access_token/refresh_token from Duende Identity Server and updates cookies
        /// </summary>
        /// <param name = "token">refresh_token that will be used to obtain a fresh access_token</param>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK with <see cref="TokensViewModel"/> data and updates cookies
        ///             ("PulseStore_webapi_auth", "auth_cookie_expiry") if correct credentials were provided
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if refresh_token or configuration is incorrect/wrong
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpGet("refresh")]
        [ProducesResponseType(typeof(TokensViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshAccessToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Refresh token must not be empty");
            }

            var httpClient = _httpClientFactory.CreateClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync(_configuration["IdentityServer:Host"]!);
            if (disco.IsError)
            {
                return BadRequest("Error while discovering Identity Server configuration");
            }

            var tokenResponse = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["TokenRequestConfiguration:ClientId"]!,
                ClientSecret = _configuration["TokenRequestConfiguration:ClientSecret"]!,
                GrantType = "refresh_token",
                RefreshToken = token,
            });

            string? tokenResponseError = AccountHelpers.GetTokenResponseError(tokenResponse);
            if (tokenResponseError is not null)
            {
                return BadRequest(tokenResponseError);
            }

            var tokensBundle = new TokensViewModel(tokenResponse.AccessToken!, tokenResponse.RefreshToken!);
            string json = JsonSerializer.Serialize(tokensBundle);

            DateTimeOffset? cookieExpiryTime =
                AccountHelpers.GetCookieExpiryTime(Request.Cookies[_configuration["CookiesConfiguration:TimeCookie:Key"]!]);
            if (cookieExpiryTime is null)
            {
                Response.Cookies.Delete(
                    _configuration["CookiesConfiguration:TimeCookie:Key"]!,
                    AccountHelpers.CreateCookieOptions(new DateTimeOffset()));
            }

            Response.Cookies.Append(
                _configuration["CookiesConfiguration:AuthCookie:Key"]!,
                AccountHelpers.GetEncodedJsonToBase64(json),
                AccountHelpers.CreateCookieOptions(cookieExpiryTime));

            return Ok(tokensBundle);
        }

        /// <summary>
        ///     Extracts access_token/refresh_token from the cookie and returns them as JSON
        /// </summary>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK with <see cref="TokensViewModel"/> data
        ///         </item>
        ///         <item>
        ///             HTTP 204 NoContent if the cookie does not exist
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if the cookie value or tokens are invalid
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpGet("retrieve-from-cookie")]
        [ProducesResponseType(typeof(TokensViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetTokensFromCookie()
        {
            string? cookie = Request.Cookies[_configuration["CookiesConfiguration:AuthCookie:Key"]!];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return NoContent();
            }

            var tokensBundle= AccountHelpers.ExtractTokensBundleFromCookie(cookie);
            if (tokensBundle is null)
            {
                return BadRequest("Invalid tokens in cookie");
            }

            return Ok(tokensBundle);
        }
    }
}