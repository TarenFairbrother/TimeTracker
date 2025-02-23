using Microsoft.AspNetCore.Identity;
using TimeTracker.Shared.Models.Account;

namespace TimeTracker.API.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
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

    public async Task AssignRole(string userName, string roleName)
    {
        if (!await this.roleManager.RoleExistsAsync(roleName))
        {
            await this.roleManager.CreateAsync(new IdentityRole(roleName));
        }
        
        var user = await this.userManager.FindByNameAsync(userName);
        await this.userManager.AddToRoleAsync(user!, roleName);
    }
}