using System.Text;
using System.Text.Json;
using IdentityModel.Client;
using PulseStore.PL.Helpers;
using PulseStore.PL.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AccountControllers
{
    [Route("api/account/[controller]")]
    [ApiExplorerSettings(GroupName = "Account/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DateTimeOffset _cookieRememberMeExpiryTime;

        public AuthController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            _cookieRememberMeExpiryTime = DateTimeOffset.Now.AddDays(7);
        }

        /// <summary>
        ///     Performs user login, appends specific cookies and returns JSON if correct credentials were provided
        /// </summary>
        /// <param name = "authModel" ><see cref="AuthRequestViewModel"/> object with email and password data</param>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK with <see cref="AuthResponseViewModel"/> data and appends cookies
        ///             ("PulseStore_webapi_auth", "auth_cookie_expiry") if correct credentials were provided
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if email, password or configuration is incorrect/wrong
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] AuthRequestViewModel authModel)
        {
            if (string.IsNullOrWhiteSpace(authModel.Email) || string.IsNullOrWhiteSpace(authModel.Password))
            {
                return BadRequest("Email and password must not be empty");
            }

            var httpClient = _httpClientFactory.CreateClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync(_configuration["IdentityServer:Host"]!);
            if (disco.IsError)
            {
                return BadRequest("Error while discovering Identity Server configuration");
            }

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["TokenRequestConfiguration:ClientId"]!,
                ClientSecret = _configuration["TokenRequestConfiguration:ClientSecret"]!,
                Scope = _configuration["TokenRequestConfiguration:Scope"]!,
                UserName = authModel.Email,
                Password = authModel.Password,
            });

            string? tokenResponseError = AccountHelpers.GetTokenResponseError(tokenResponse);
            if (tokenResponseError is not null)
            {
                return BadRequest(tokenResponseError);
            }

            var tokensBundle = new TokensViewModel(tokenResponse.AccessToken!, tokenResponse.RefreshToken!);
            string json = JsonSerializer.Serialize(tokensBundle);

            string? cookieExpiryTimeInSeconds = null;
            DateTimeOffset? cookieExpiryTime = authModel.RememberMe ? _cookieRememberMeExpiryTime : null;
            if (cookieExpiryTime.HasValue)
            {
                cookieExpiryTimeInSeconds = cookieExpiryTime.Value.ToUnixTimeSeconds().ToString();
                Response.Cookies.Append(
                    _configuration["CookiesConfiguration:TimeCookie:Key"]!,
                    cookieExpiryTimeInSeconds,
                    AccountHelpers.CreateCookieOptions(cookieExpiryTime));
            }

            Response.Cookies.Append(
                _configuration["CookiesConfiguration:AuthCookie:Key"]!,
                AccountHelpers.GetEncodedJsonToBase64(json),
                AccountHelpers.CreateCookieOptions(cookieExpiryTime));

            return Ok(new AuthResponseViewModel(tokensBundle, cookieExpiryTimeInSeconds));
        }

        /// <summary>
        ///     Performs user logout, put access_token into blacklist on Duende Identity Server
        ///     and deletes specific cookies
        /// </summary>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK if access_token was put into blacklist and specific cookies were deleted
        ///         </item>
        ///         <item>
        ///             HTTP 204 NoContent if the cookie does not exist
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if the cookie value or tokens are invalid or failed to put
        ///             access_token into blacklist on Duende Identity Server
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpGet("logout")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            string? cookie = Request.Cookies[_configuration["CookiesConfiguration:AuthCookie:Key"]!];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return NoContent();
            }

            // Delete cookies (i.e. perform log out) in the browser no matter what contents are there
            Response.Cookies.Delete(
                _configuration["CookiesConfiguration:AuthCookie:Key"]!,
                AccountHelpers.CreateCookieOptions(new DateTimeOffset()));

            Response.Cookies.Delete(
                _configuration["CookiesConfiguration:TimeCookie:Key"]!,
                AccountHelpers.CreateCookieOptions(new DateTimeOffset()));

            var tokensBundle= AccountHelpers.ExtractTokensBundleFromCookie(cookie);
            if (tokensBundle is null)
            {
                return Ok("Deleted cookies successfully. Invalid tokens in the AuthCookie");
            }

            var httpClient = _httpClientFactory.CreateClient();

            string tokenBlacklistUrl = $"{_configuration["IdentityServer:Host"]!}/connect/token/blacklist";
            var requestContent = new StringContent($"\"{tokensBundle.AccessToken}\"", Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(tokenBlacklistUrl, requestContent);

                string responseContent = await response.Content.ReadAsStringAsync();
                return Ok($"Deleted cookies successfully. AccessToken: {responseContent}");
            }
            catch (Exception ex)
            {
                return Ok($"Deleted cookies successfully. Exception: {ex.Message}");
            }
        }
    }
}