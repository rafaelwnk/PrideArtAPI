using System.ComponentModel.DataAnnotations;

namespace PrideArtAPI.ViewModels.Accounts;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirme a senha.")]
    [Compare("Password", ErrorMessage = "As senhas não são iguais.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}