using System.ComponentModel.DataAnnotations;
namespace BlazorismChat.ClientLibraries.DTOs;

public class LoginDTO
{
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

    public bool RememberMe { get; set; }
}

public class RegisterDTO
{
    [Display(Name = "Username", Prompt = "Username...")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long")]
    [MaxLength(60, ErrorMessage = "{0} must be at most {1} characters long")]
    public string UserName { get; set; }

    [Display(Name = "Email", Prompt = "example@gmail.com")]
    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress(ErrorMessage = "Invalid {0}")]
    public string Email { get; set; }

    [Display(Name = "Password", Prompt = "Password...")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(8, ErrorMessage = "{0} must be at least {1} characters long")]
    [MaxLength(60, ErrorMessage = "{0} must be at most {1} characters long")]
    public string Password { get; set; }

    [Display(Name = "Confirm Password", Prompt = "Repeat password")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(8, ErrorMessage = "{0} must be at least {1} characters long")]
    [MaxLength(60, ErrorMessage = "{0} must be at most {1} characters long")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}