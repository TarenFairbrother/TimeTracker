using TimeTracker.Shared.Models.Account;
using TimeTracker.Shared.Models.Login;

namespace TimeTracker.Client.Services;

public interface IAuthservice
{
    Task Register(AccountRegistrationRequest request);
    Task Login(LoginRequest request);
    Task Logout();
}