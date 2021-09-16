namespace BlazorismChat.ClientLibraries.DTOs;

public class AuthenticationResponse
{
    public string Token { get; set; }
}

public class AuthenticationRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}