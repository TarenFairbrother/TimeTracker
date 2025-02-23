using TimeTracker.Shared.Models.Login;

namespace TimeTracker.API.Services;

public interface ILoginService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}