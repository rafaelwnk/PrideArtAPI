using System.ComponentModel.DataAnnotations;

namespace PrideArtAPI.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o usuário.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    public string Password { get; set; } = string.Empty;
}