using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrideArtAPI.Exceptions;
using PrideArtAPI.Extensions;
using PrideArtAPI.Interfaces;
using PrideArtAPI.Models;
using PrideArtAPI.ViewModels;
using PrideArtAPI.ViewModels.Posts;

namespace PrideArtAPI.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [Authorize]
    [HttpPost("v1/posts")]
    public async Task<IActionResult> CreatePostAsync([FromBody] CreatePostViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var post = await _postRepository.CreatePostAsync(model, User.Identity!.Name!);
            return Ok(new ResultViewModel<dynamic>(new
            {
                post,
                message = "Post criado com sucesso!"
            }));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível criar o post."));
        }
        catch (IOException)
        {
            return StatusCode(500, "Erro ao salvar a imagem.");
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/posts/explore")]
    public async Task<IActionResult> GetAllPostsAsync()
    {
        try
        {
            var posts = await _postRepository.GetAllPostsAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<List<Post>>(posts));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/posts/{username}")]
    public async Task<IActionResult> GetAllPostsByUsername([FromRoute] string username)
    {
        try
        {
            var posts = await _postRepository.GetAllPostsByUsernameAsync(username);
            return Ok(new ResultViewModel<List<Post>>(posts));
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
    [HttpPut("v1/posts/{id:int}")]
    public async Task<IActionResult> EditPostAsync([FromRoute] int id, [FromBody] EditPostViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var post = await _postRepository.EditPostAsync(model, id, User.Identity!.Name!);
            return Ok(new ResultViewModel<dynamic>(new
            {
                post,
                message = "Post editado com sucesso!"
            }));
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível editar o post."));
        }
        catch (IOException)
        {
            return StatusCode(500, "Erro ao salvar a imagem.");
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpDelete("v1/posts/{id:int}")]
    public async Task<IActionResult> DeletePostByIdAsync([FromRoute] int id)
    {
        try
        {
            var post = await _postRepository.DeletePostByIdAsync(id, User.Identity!.Name!);
            return Ok(new ResultViewModel<dynamic>(new
            {
                post,
                message = "Post deletado com sucesso!"
            }));
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível deletar o post."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/posts/following")]
    public async Task<IActionResult> GetFollowingPostsAsync()
    {
        try
        {
            var posts = await _postRepository.GetFollowingPostsAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<List<Post>>(posts));
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
    [HttpPost("v1/posts/like/{id:int}")]
    public async Task<IActionResult> LikePostAsync([FromRoute] int id)
    {
        try
        {
            var post = await _postRepository.LikePostAsync(User.Identity!.Name!, id);
            return Ok(new ResultViewModel<Post>(post));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Não foi possível curtir o post."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }

    [Authorize]
    [HttpGet("v1/posts/liked")]
    public async Task<IActionResult> GetLikedPostsAsync()
    {
        try
        {
            var posts = await _postRepository.GetLikedPostsAsync(User.Identity!.Name!);
            return Ok(new ResultViewModel<List<Post>>(posts));
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
    [HttpDelete("v1/posts/unlike/{id:int}")]
    public async Task<IActionResult> UnlikePostAsync([FromRoute] int id)
    {
        try
        {
            var post = await _postRepository.UnlikePostAsync(User.Identity!.Name!, id);
            return Ok(new ResultViewModel<Post>(post));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno."));
        }
    }
}