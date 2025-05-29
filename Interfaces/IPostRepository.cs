using PrideArtAPI.Models;
using PrideArtAPI.ViewModels.Posts;

namespace PrideArtAPI.Interfaces;

public interface IPostRepository
{
    Task<Post> CreatePostAsync(CreatePostViewModel model, string username);
    Task<List<Post>> GetAllPostsAsync(string username);
    Task<List<Post>> GetAllPostsByUsernameAsync(string username);
    Task<Post> EditPostAsync(EditPostViewModel model, int id, string username);
    Task<Post> DeletePostByIdAsync(int id, string username);
}