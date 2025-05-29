using System.ComponentModel.DataAnnotations;

namespace PrideArtAPI.ViewModels.Posts;

public class CreatePostViewModel
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "A imagem é obrigatória.")]
    public string Image { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Description { get; set; } = string.Empty;
}