namespace BlazorismChat.ClientLibraries.DTOs;

public class AuthenticationResponse
{
    public string Token { get; set; }
}

public class AuthenticationRequest
{
    public AuthenticationRequest(string userName, string password) : this() 
    {
        UserName = userName;
        Password = password;
    }

    public AuthenticationRequest()
    {
        
    }

    public string UserName { get; set; }

    public string Password { get; set; }
}