using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Account;

namespace ShipmentManagement.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        private readonly ILogActionService _logActionService;

        // Session Keys
        public const string SESSION_EMAIL = "UserEmail";
        public const string SESSION_USERNAME = "UserName";
        public const string SESSION_ROLE = "UserRole";
        public const string SESSION_USERID = "UserId";

        public AuthService(IUserRepository userRepo,ILogActionService logActionService)
        {
            _userRepo = userRepo;
            _logActionService = logActionService;
        }

        public async Task<bool> LoginAsync(LoginViewModel model, ISession session)
        {
            try
            {
            var user = await _userRepo.ValidateUserAsync(
                model.Email, model.Password);

            if (user == null) return false;

            // Set session values
            session.SetString(SESSION_EMAIL, user.Email);
            session.SetString(SESSION_USERNAME, user.Username);
            session.SetString(SESSION_ROLE, user.Role);
            session.SetInt32(SESSION_USERID, user.Id);

            // Update last login timestamp
            await _userRepo.UpdateLastLoginAsync(user.Id);

            return true;
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("AuthService.LoginAsync", "Login", model.Email.ToString(), $"Error while Login customer : {ex.Message}");
                return false;
            }
        }

        public void Logout(ISession session)
        {
            session.Clear();
        }

        public bool IsLoggedIn(ISession session)
        {
            return !string.IsNullOrEmpty(
                session.GetString(SESSION_EMAIL));
        }
    }
}