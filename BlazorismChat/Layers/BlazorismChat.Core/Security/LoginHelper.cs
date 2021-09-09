using BlazorismChat.DbLayer.Entities.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlazzingExam.Core.Server.Security
{
    public static class LoginHelper
    {
        /// <summary>
        /// Web custom login
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="user">User you want to login</param>
        /// <param name="rememberMe">Is Persistent</param>
        /// <returns>Login</returns>
        public static async Task LoginAsync(this HttpContext context, User user, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
                new Claim(nameof(user.IdentityCode), user.IdentityCode),
                new Claim(nameof(user.ActiveCode), user.ActiveCode),
                new Claim("Rem", rememberMe.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
            };
            
            await context.SignInAsync(principal, properties);
        }

        /// <summary>
        /// Web Custom logout
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Logout</returns>
        public static async Task LogoutAsync(this HttpContext context) =>
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}