using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorismChat.ClientLibraries.ClientServices.Interfaces;
using BlazorismChat.ClientLibraries.Convertors;
using BlazorismChat.ClientLibraries.DTOs;

namespace BlazorismChat.ClientLibraries.ClientServices;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public UserService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    public async Task<bool> Login(LoginDTO loginDTO)
    {
        var requestData = new AuthenticationRequest(loginDTO.UserName, loginDTO.Password);


        var response = await _httpClient.PostAsJsonAsync(ApiLocations.Account+"AuthenticateJWT", requestData);
        try
        {
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

                if (responseData == null)
                    return false;
                    
                await _localStorageService.SetItemAsync<string>("JWT_TOKEN", responseData.Token);
                return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
}