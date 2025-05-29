namespace PrideArtAPI.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<User> UsersLiked { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
}