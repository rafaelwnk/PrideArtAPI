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
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        return user;
    }
}