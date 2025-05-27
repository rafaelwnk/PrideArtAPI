using System.Text.Json.Serialization;

namespace PrideArtAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    public string Identity { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public List<Post> Posts { get; set; } = new();
    public List<Post> LikedPosts { get; set; } = new();
    public List<User> FollowedUsers { get; set; } = new();
}
