using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrideArtAPI.Exceptions;
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
            return StatusCode(400, new ResultViewModel<string>("Não foi possível cadastrar o usuário."));
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

    [HttpPatch("v1/accounts/reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await _accountRepository.ResetPasswordAsync(model);
            return Ok(new ResultViewModel<dynamic>(new
            {
                user,
                message = "Senha alterada com sucesso!"
            }));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível alterar a senha do usuário."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }

    }

    [Authorize]
    [HttpPost("v1/accounts/refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromServices] TokenService tokenService)
    {
        try
        {
            var user = await _accountRepository.GetUserByUsernameAsync(User.Identity!.Name!);
            var token = tokenService.Create(user);
            return Ok(new ResultViewModel<string>(token, null!));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/accounts/me")]
    public async Task<IActionResult> GetLoggedUserAsync()
    {
        try
        {
            var user = await _accountRepository.GetUserByUsernameAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<User>(user));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpPut("v1/accounts/edit-profile")]
    public async Task<IActionResult> EditProfileAsync([FromBody] EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await _accountRepository.EditProfileAsync(User.Identity!.Name!, model);
            return Ok(new ResultViewModel<dynamic>(new
            {
                user,
                message = "Perfil atualizado com sucesso!"
            }));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível atualizar o perfil."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpDelete("v1/accounts")]
    public async Task<IActionResult> DeleteProfileAsync()
    {
        try
        {
            var user = await _accountRepository.DeleteProfileAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<dynamic>(new
            {
                user,
                message = "Usuário excluído com sucesso!"
            }));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível deletar o perfil."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/accounts/{username}")]
    public async Task<IActionResult> GetUserByUsernameAsync([FromRoute] string username)
    {
        try
        {
            var user = await _accountRepository.GetUserByUsernameAsync(username);
            return Ok(new ResultViewModel<User>(user));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/accounts/explore")]
    public async Task<IActionResult> GetUsersAsync()
    {
        try
        {
            var users = await _accountRepository.GetUsersAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<List<User>>(users));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpPost("v1/accounts/{followedUsername}/follow")]
    public async Task<IActionResult> FollowUserAsync([FromRoute] string followedUsername)
    {
        try
        {
            var user = await _accountRepository.FollowUserAsync(User.Identity!.Name!, followedUsername);
            return Ok(new ResultViewModel<User>(user));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResultViewModel<string>(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }

    }

    [Authorize]
    [HttpGet("v1/accounts/following")]
    public async Task<IActionResult> GetFollowingUsersAsync()
    {
        try
        {
            var users = await _accountRepository.GetFollowingUsersAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<List<User>>(users));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpDelete("v1/accounts/{unfollowedUsername}/unfollow")]
    public async Task<IActionResult> UnfollowUserAsync([FromRoute] string unfollowedUsername)
    {
        try
        {
            var user = await _accountRepository.UnfollowUserAsync(User.Identity!.Name!, unfollowedUsername);
            return Ok(new ResultViewModel<User>(user));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResultViewModel<string>(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível deixar de seguir o usuário."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }
}