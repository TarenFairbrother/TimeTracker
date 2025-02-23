using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TimeTracker.Shared.Models.Login;

namespace TimeTracker.API.Services;

public class LoginService : ILoginService
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly IConfiguration config;

    public LoginService(SignInManager<User> signInManager, IConfiguration config, UserManager<User> userManager)
    {
        this.signInManager = signInManager;
        this.config = config;
        this.userManager = userManager;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var result = await this.signInManager.PasswordSignInAsync(loginRequest.UserName,
            loginRequest.Password, false, false);

        if (!result.Succeeded)
        {
            return new LoginResponse(false, "Email or password is incorrect.", null);
        }
        
        var user = await this.userManager.FindByNameAsync(loginRequest.UserName);
        if (user == null)
        {
            return new LoginResponse(false, "User doesn't exist.", null);
        }
        
        var roles = await this.userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, loginRequest.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        }.ToList();
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(this.config["JwtSecurityKey"]!));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expiry = DateTime.Now.AddDays(Convert.ToInt32(this.config["JwtExpiryInDays"]!));

        var token = new JwtSecurityToken(
            issuer: this.config["JwtIssuer"]!,
            audience: this.config["JwtAudience"]!,
            claims: claims,
            expires: expiry,
            signingCredentials: creds);
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new LoginResponse(true, null, jwt);
    }
}