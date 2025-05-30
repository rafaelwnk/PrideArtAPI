using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using PrideArtAPI.Data;
using PrideArtAPI.Exceptions;
using PrideArtAPI.Interfaces;
using PrideArtAPI.Models;
using PrideArtAPI.ViewModels.Accounts;
using SecureIdentity.Password;

namespace PrideArtAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;

    public AccountRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterAsync(RegisterViewModel model)
    {
        var user = new User
        {
            Name = model.Name,
            Username = model.Username,
            Email = model.Email,
            Identity = model.Identity,
            Password = PasswordHasher.Hash(model.Password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> LoginAsync(LoginViewModel model)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == model.Username);

        if (user == null)
            throw new InvalidCredentialException("Usuário ou senha inválidos.");

        if (!PasswordHasher.Verify(user.Password, model.Password))
            throw new InvalidCredentialException("Usuário ou senha inválidos.");

        return user;
    }

    public async Task<User> ResetPasswordAsync(ResetPasswordViewModel model)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            throw new EmailNotFoundException();

        user.Password = PasswordHasher.Hash(model.Password);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        return user;
    }

    public async Task<User> EditProfileAsync(string username, EditProfileViewModel model)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        user.Name = model.Name;
        user.Email = model.Email;
        user.Identity = model.Identity;
        user.Bio = model.Bio;
        user.Image = model.Image;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> DeleteProfileAsync(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> FollowUserAsync(string username, string followedUsername)
    {
        if (username == followedUsername)
            throw new InvalidOperationException("Você não pode seguir a si mesmo.");

        var user = await _context.Users
            .Include(x => x.Following)
            .FirstOrDefaultAsync(x => x.Username == username);

        var followedUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == followedUsername);

        if (user == null || followedUser == null)
            throw new UserNotFoundException();

        if (user.Following.Any(x => x.Id == followedUser.Id))
            throw new InvalidOperationException("Você já está seguindo este usuário.");

        user.Following.Add(followedUser);
        await _context.SaveChangesAsync();

        return followedUser;
    }

    public async Task<List<User>> GetFollowingUsersAsync(string username)
    {
        var user = await _context.Users
            .Include(x => x.Following)
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        return user.Following;
    }

    public async Task<User> UnfollowUserAsync(string username, string unfollowedUsername)
    {
        if (username == unfollowedUsername)
            throw new InvalidOperationException("Você não pode deixar de seguir a si mesmo.");

        var user = await _context.Users
            .Include(x => x.Following)
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        var unfollowedUser = user.Following.FirstOrDefault(x => x.Username == unfollowedUsername);

        if (unfollowedUser == null)
            throw new InvalidOperationException("Você não segue este usuário.");

        user.Following.Remove(unfollowedUser);
        await _context.SaveChangesAsync();

        return unfollowedUser;
    }
}