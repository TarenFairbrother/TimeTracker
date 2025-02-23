using Microsoft.AspNetCore.Mvc;
using TimeTracker.Shared.Models.Account;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpPost]
    public async Task<ActionResult<AccountRegistrationResponse>> Register(AccountRegistrationRequest request)
    {
        var result = await this.accountService.RegisterAsync(request);
        
        return Ok(result);
    }

    [HttpPost("role")]
    public async Task<IActionResult> AssignRole(string userName, string roleName)
    {
       await this.accountService.AssignRole(userName, roleName);
       return Ok();
    }
}