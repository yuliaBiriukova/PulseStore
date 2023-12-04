using System.Text.Json.Serialization;

namespace PulseStore.PL.ViewModels.Account
{
    public record AuthResponseViewModel(
        TokensViewModel Tokens,
        [property: JsonPropertyName("cookie_expiry_time")]string? CookieExpiryTime);
}