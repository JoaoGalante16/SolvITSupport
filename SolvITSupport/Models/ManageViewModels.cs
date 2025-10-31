using Microsoft.AspNetCore.Http; // Para IFormFile (upload de foto)
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    // ViewModel para a ABA "PERFIL"
    // (Baseado no seu ApplicationUser.cs)
    public class ProfileViewModel
    {
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [EmailAddress]
        public string? Email { get; set; } // Apenas para exibição

        [Display(Name = "Departamento")]
        public string? Departamento { get; set; }

        [Display(Name = "Bio")]
        [DataType(DataType.MultilineText)]
        public string? Bio { get; set; }

        // Para exibir a foto atual (lendo a propriedade 'Foto' do ApplicationUser)
        [Display(Name = "Foto de Perfil Atual")]
        public string? ProfilePictureUrl { get; set; }

        // Para o upload de uma nova foto
        [Display(Name = "Alterar Foto")]
        public IFormFile? ProfilePictureFile { get; set; }

        public string? StatusMessage { get; set; }
    }

    // ViewModel "CONTAINER" para a PÁGINA INTEIRA
    public class SettingsViewModel
    {
        public ProfileViewModel Profile { get; set; }
        public ChangePasswordViewModel Security { get; set; }
        public string ActiveTab { get; set; }

        public SettingsViewModel()
        {
            Profile = new ProfileViewModel();
            Security = new ChangePasswordViewModel();
            ActiveTab = "Perfil"; // Aba padrão
        }
    }

    // ViewModel para a ABA "SEGURANÇA"
    // (Este é o seu ViewModel original do arquivo ManageViewModels.cs)
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não coincidem.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? StatusMessage { get; set; }
    }

    // O seu IndexViewModel original 
    // não será mais usado pela View Index, mas pode ser mantido.
    public class IndexViewModel
    {
        public string Username { get; set; }
        public string NomeCompleto { get; set; }
        public string Telefone { get; set; }
    }
}