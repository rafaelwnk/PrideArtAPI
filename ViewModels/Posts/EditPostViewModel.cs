using System.ComponentModel.DataAnnotations;

namespace PrideArtAPI.ViewModels.Posts;

public class EditPostViewModel
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Title { get; set; } = string.Empty;
    public string? Image { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Description { get; set; } = string.Empty;
}