using Microsoft.AspNetCore.Mvc;
using TimeTracker.Shared.Models.Login;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class LoginController : ControllerBase
{
    private readonly ILoginService loginService;

    public LoginController(ILoginService loginService)
    {
        this.loginService = loginService;
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var result = await this.loginService.LoginAsync(loginRequest);
        return Ok(result);
    }
}