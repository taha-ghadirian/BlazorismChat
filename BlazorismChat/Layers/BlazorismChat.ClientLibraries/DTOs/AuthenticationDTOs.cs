using System.ComponentModel.DataAnnotations;

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

    [Display(Name = "Username", Prompt = "Username...")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long")]
    [MaxLength(60, ErrorMessage = "{0} must be at most {1} characters long")]
    public string UserName { get; set; }

    [Display(Name = "Password", Prompt = "Password...")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(8, ErrorMessage = "{0} must be at least {1} characters long")]
    [MaxLength(60, ErrorMessage = "{0} must be at most {1} characters long")]
    public string Password { get; set; }
}