using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PrideArtAPI.Data;
using PrideArtAPI.Exceptions;
using PrideArtAPI.Interfaces;
using PrideArtAPI.Models;
using PrideArtAPI.ViewModels.Posts;

namespace PrideArtAPI.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DataContext _context;

    public PostRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Post> CreatePostAsync(CreatePostViewModel model, string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        var fileName = $"{Guid.NewGuid()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Image, "");
        var bytes = Convert.FromBase64String(data);

        await System.IO.File.WriteAllBytesAsync($"wwwroot/images/posts-images/{fileName}", bytes);

        var post = new Post
        {
            Title = model.Title,
            Image = $"{Configuration.UrlPostImage}{fileName}",
            Description = model.Description,
            UserId = user.Id
        };

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        return post;
    }

    public async Task<List<Post>> GetAllPostsAsync(string username)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        var posts = await _context.Posts
            .AsNoTracking()
            .Where(x => x.UserId != user.Id)
            .ToListAsync();

        return posts;
    }

    public async Task<List<Post>> GetAllPostsByUsernameAsync(string username)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            throw new UserNotFoundException();

        var posts = await _context.Posts
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .ToListAsync();

        return posts;
    }

    public async Task<Post> EditPostAsync(EditPostViewModel model, int id, string username)
    {
        var post = await _context.Posts
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (post == null)
            throw new PostNotFoundException();

        if (post.User!.Username != username)
            throw new UserNotFoundException();

        if (!string.IsNullOrEmpty(model.Image))
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Image, "");
            var bytes = Convert.FromBase64String(data);

            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/posts-images/{fileName}", bytes);
            post.Image = $"{Configuration.UrlPostImage}{fileName}";
        }

        post.Title = model.Title;
        post.Description = model.Description;
        post.LastUpdate = DateTime.UtcNow;

        _context.Posts.Update(post);
        await _context.SaveChangesAsync();

        return post;
    }

    public async Task<Post> DeletePostByIdAsync(int id, string username)
    {
        var post = await _context.Posts
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (post == null)
            throw new PostNotFoundException();

        if (post.User!.Username != username)
            throw new UserNotFoundException();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return post;
    }

}