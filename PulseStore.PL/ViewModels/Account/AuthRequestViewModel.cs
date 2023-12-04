namespace PulseStore.PL.ViewModels.Account
{
    public record AuthRequestViewModel(
        string Email,
        string Password,
        bool RememberMe);
}