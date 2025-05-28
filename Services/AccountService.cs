using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using PrideArtAPI.Data;
using PrideArtAPI.Interfaces;
using PrideArtAPI.Models;
using PrideArtAPI.ViewModels.Accounts;
using SecureIdentity.Password;

namespace PrideArtAPI.Services;

public class AccountService : IAccountService
{
    private readonly DataContext _context;

    public AccountService(DataContext context)
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
}