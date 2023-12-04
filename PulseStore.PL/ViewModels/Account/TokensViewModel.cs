using System.Text.Json.Serialization;

namespace PulseStore.PL.ViewModels.Account
{
    public record TokensViewModel(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("refresh_token")] string RefreshToken);
}