using PrideArtAPI.Models;
using PrideArtAPI.ViewModels.Accounts;

namespace PrideArtAPI.Interfaces;

public interface IAccountRepository
{
    Task<User> RegisterAsync(RegisterViewModel model);
    Task<User> LoginAsync(LoginViewModel model);
    
}