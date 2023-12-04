using System.Text;
using System.Text.Json;
using IdentityModel.Client;
using PulseStore.PL.ViewModels.Account;

namespace PulseStore.PL.Helpers
{
    public static class AccountHelpers
    {
        public static CookieOptions CreateCookieOptions(DateTimeOffset? expires)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                SameSite = SameSiteMode.None,
                Secure = true,
                IsEssential = true,
            };
        }

        public static string GetEncodedJsonToBase64(string json)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(jsonBytes);
        }

        public static TokensViewModel? ExtractTokensBundleFromCookie(string cookie)
        {
            try
            {
                byte[] jsonBytes = Convert.FromBase64String(cookie);
                string json = Encoding.UTF8.GetString(jsonBytes);
                return JsonSerializer.Deserialize<TokensViewModel>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTimeOffset? GetCookieExpiryTime(string? timeCookie)
        {
            if (!string.IsNullOrWhiteSpace(timeCookie))
            {
                try
                {
                    return DateTimeOffset.FromUnixTimeSeconds(long.Parse(timeCookie));
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public static string? GetTokenResponseError(TokenResponse? tokenResponse)
        {
            if (tokenResponse is null)
            {
                return "Error while requesting access token: token response is null";
            }

            if (tokenResponse.IsError)
            {
                return $"Error while requesting access token: {tokenResponse.ErrorDescription ?? tokenResponse.Error}";
            }

            if (tokenResponse.AccessToken is null || tokenResponse.RefreshToken is null)
            {
                return "Access token and refresh token must not be null";
            }

            return null;
        }
    }
}