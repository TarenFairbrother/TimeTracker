using System.Net.Http.Json;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TimeTracker.Shared.Models.Account;
using TimeTracker.Shared.Models.Login;

namespace TimeTracker.Client.Services;

public class AuthService : IAuthservice
{
    private readonly HttpClient httpClient;
    private readonly IToastService toastService;
    private readonly NavigationManager navigationManager;
    private readonly ILocalStorageService localStorage;
    private readonly AuthenticationStateProvider authStateProvider;

    public AuthService(HttpClient httpClient, IToastService toastService, NavigationManager navigationManager, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        this.httpClient = httpClient;
        this.toastService = toastService;
        this.navigationManager = navigationManager;
        this.localStorage = localStorage;
        this.authStateProvider = authStateProvider;
    }

    public async Task Register(AccountRegistrationRequest request)
    {
       var result = await this.httpClient.PostAsJsonAsync("api/account", request);
       if (result != null)
       {
           var response = await result.Content.ReadFromJsonAsync<AccountRegistrationResponse>();
           if (!response.IsSuccessful && response.Errors != null)
           {
               foreach (var error in response.Errors)
               {
                   this.toastService.ShowError(error);
               }
           }
           else if (!response.IsSuccessful)
           {
               this.toastService.ShowError("An unexpected error occured");
           }
           else
           {
               this.toastService.ShowSuccess("Account registered successfully");
           }
       }
       
    }

    public async Task Login(LoginRequest request)
    {
        var result = await this.httpClient.PostAsJsonAsync("api/login", request);
        if (result != null)
        {
            var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
            if (!response.Success && response.Error != null)
            {
                this.toastService.ShowError(response.Error);
            }
            else if (!response.Success)
            {
                this.toastService.ShowError("An unexpected error occured");
            }
            else
            {
                if (response.Token != null)
                {
                    await this.localStorage.SetItemAsStringAsync("authToken", response.Token);
                    await this.authStateProvider.GetAuthenticationStateAsync();
                }
                this.navigationManager.NavigateTo("timeentries");
            }
        }
    }

    public async Task Logout()
    {
        await this.localStorage.RemoveItemAsync("authToken");
        await this.authStateProvider.GetAuthenticationStateAsync();
        this.navigationManager.NavigateTo("/login");
    }
}