using PrideArtAPI.Models;
using PrideArtAPI.ViewModels.Accounts;

namespace PrideArtAPI.Interfaces;

public interface IAccountRepository
{
    Task<User> RegisterAsync(RegisterViewModel model);
    Task<User> LoginAsync(LoginViewModel model);
    Task<User> ResetPasswordAsync(ResetPasswordViewModel model);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> EditProfileAsync(string username, EditProfileViewModel model);
    Task<User> DeleteProfileAsync(string username);
    Task<List<User>> GetUsersAsync(string username);
    Task<User> FollowUserAsync(string username, string followedUsername);
    Task<List<User>> GetFollowingUsersAsync(string username);
    Task<User> UnfollowUserAsync(string username, string unfollowedUsername);
}