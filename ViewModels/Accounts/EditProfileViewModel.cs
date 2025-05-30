using System.ComponentModel.DataAnnotations;

namespace PrideArtAPI.ViewModels.Accounts;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Por favor, informe como você se identifica.")]
    public string Identity { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public string? Image { get; set; }
}