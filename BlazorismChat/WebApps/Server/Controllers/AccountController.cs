using BlazorismChat.Core.ServerServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlazzingExam.Core.Server.Security;
using BlazorismChat.DbLayer.Entities.Users;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BlazorismChat.ClientLibraries.DTOs;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace BlazorismChat.Server.Controllers;
[ApiController]
[Route("api/v1/[controllr]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AccountController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        var result = await _userService.LoginUser(loginDTO.UserName, loginDTO.Password);

        if (result != null)
        {
            await HttpContext.LoginAsync(result, loginDTO.RememberMe);
            return Ok(result);
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        var result = await _userService.RegisterUser(registerDTO);

        if (result != null)
        {
            return Ok(result);
        }

        return Unauthorized();
    }

    [HttpPost("authenticatejwt")]
    public async Task<ActionResult<AuthenticationResponse>> AuthenticateJWT(AuthenticationRequest authenticationRequest)
    {
        string token = string.Empty;

        //checking if the user exists in the database
        var loggedInUser = await _userService.LoginUser(authenticationRequest.UserName, authenticationRequest.Password);

        if (loggedInUser != null)
        {
            //generating the token
            token = LoginHelper.GenerateJwtToken(loggedInUser, _configuration);
        }

        return new AuthenticationResponse() { Token = token };
    }

    [HttpPost("getuserbyjwt")]
    public async Task<ActionResult<User?>?> GetUserByJWT([FromBody] string jwtToken)
    {
        try
        {
            //getting the secret key
            string secretKey = _configuration["JWTSettings:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            //preparing the validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            //validating the token
            var principle = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = (JwtSecurityToken)securityToken;

            if (jwtSecurityToken != null
                && jwtSecurityToken.ValidTo > DateTime.Now
                && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                //returning the user if found
                var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return await _userService.GetUserById(int.Parse(userId ?? ""));
            }
        }
        catch (Exception ex)
        {
            //logging the error and returning null
            Console.WriteLine("Exception : " + ex.Message);
            return null;
        }
        //returning null if token is not validated
        return null;
    }
}
