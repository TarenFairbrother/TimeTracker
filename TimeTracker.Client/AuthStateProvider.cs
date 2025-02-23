using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace TimeTracker.Client;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorage;

    public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        this.httpClient = httpClient;
        this.localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await this.localStorage.GetItemAsync<string>("authToken");
        AuthenticationState authState;

        if (string.IsNullOrWhiteSpace(authToken))
        {
            this.httpClient.DefaultRequestHeaders.Authorization = null;
            authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        else
        {
            this.httpClient.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Bearer", authToken);
            authState = new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt")));
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
        
        return authState;
    }
    
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split(".")[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer
            .Deserialize<Dictionary<string, object>>(jsonBytes);
        
        var claims = new List<Claim>();

        foreach (var kvp in keyValuePairs!)
        {
            if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    claims.Add(new Claim(kvp.Key, item.ToString()));
                }
            }
            else
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()!));
            }
        }

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}