using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpPost("v1/accounts/register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await _accountRepository.RegisterAsync(model);
            return Ok(new ResultViewModel<dynamic>(new
            {
                user,
                message = "Cadastro realizado com sucesso!"
            }));
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
            var user = await _accountRepository.LoginAsync(model);
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