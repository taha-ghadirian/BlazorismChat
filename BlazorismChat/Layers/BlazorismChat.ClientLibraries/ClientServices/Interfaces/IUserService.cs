using System.Threading.Tasks;
using BlazorismChat.ClientLibraries.DTOs;

namespace BlazorismChat.ClientLibraries.ClientServices.Interfaces;

public interface IUserService
{
    Task<bool> Login(LoginDTO loginDTO);
}