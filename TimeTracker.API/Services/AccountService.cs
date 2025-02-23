using Microsoft.AspNetCore.Identity;
using TimeTracker.Shared.Models.Account;

namespace TimeTracker.API.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> userManager;

    public AccountService(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<AccountRegistrationResponse> RegisterAsync(AccountRegistrationRequest request)
    {
        var newUser = new User { UserName = request.UserName, Email = request.Email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new AccountRegistrationResponse(false, errors);
        }
        
        return new AccountRegistrationResponse(true, null);
    }
}