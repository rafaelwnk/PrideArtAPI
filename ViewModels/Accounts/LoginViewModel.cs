using System.ComponentModel.DataAnnotations;

namespace PrideArtAPI.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "O usuário é obrigatório.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;
}