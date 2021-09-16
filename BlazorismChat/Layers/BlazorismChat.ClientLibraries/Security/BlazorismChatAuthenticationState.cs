using System;
using System.Security.AccessControl;
using System.Security.Claims;
using Blazored.LocalStorage;
using BlazorismChat.DbLayer.Entities.Users;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace BlazorismChat.ClientLibraries.Security;

public class BlazorismChatAuthenticationState : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;

    public BlazorismChatAuthenticationState(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetUserByJWTAsync();

        if (currentUser != null && currentUser.Email != null)
        {
            //create a claims
            var claimEmailAddress = new Claim(ClaimTypes.Name, currentUser.Email);
            var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.UserId));

            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { claimEmailAddress, claimNameIdentifier }, "serverAuth");
            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationState(claimsPrincipal);
        }
        else
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<User?> GetUserByJWTAsync()
    {
        //pulling the token from localStorage
        var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
        if (jwtToken == null) return null;

        // TODO: complete this method
        return null;
    }
}