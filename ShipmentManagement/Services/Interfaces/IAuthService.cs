using ShipmentManagement.ViewModels.Account;

namespace ShipmentManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginViewModel model, ISession session);
        void Logout(ISession session);
        bool IsLoggedIn(ISession session);
    }
}