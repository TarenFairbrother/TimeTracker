using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TimeTracker.API.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<User> userManager;

    public UserContextService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
    }

    public string? GetUserId()
    {
        return this.httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public Task<User?> GetUserAsync()
    {
        var httpContextUser = this.httpContextAccessor.HttpContext?.User;
        if (httpContextUser == null)
        {
            return null;
        }
        
        return this.userManager.GetUserAsync(httpContextUser);
    }
}