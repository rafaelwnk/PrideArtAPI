using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrideArtAPI.Extensions;
using PrideArtAPI.Interfaces;
using PrideArtAPI.Models;
using PrideArtAPI.Services;
using PrideArtAPI.ViewModels;
using PrideArtAPI.ViewModels.Accounts;

namespace PrideArtAPI.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("v1/accounts/register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await _accountService.RegisterAsync(model);
            return Ok(new ResultViewModel<User>(user));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível cadastrar o usuário"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model, [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await _accountService.LoginAsync(model);
            var token = tokenService.Create(user);
            return Ok(new ResultViewModel<string>(token, null!));
        }
        catch (InvalidCredentialException ex)
        {
            return Unauthorized(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }
}