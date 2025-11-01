using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Display(Name = "CPF")]
        public string? CPF { get; set; }

        [Display(Name = "Cargo")]
        public string? Cargo { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres de comprimento.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A Senha e a Senha de confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Perfil/Role")]
        [Required(ErrorMessage = "O perfil é obrigatório.")]
        public string RoleName { get; set; } // Armazena o NOME do perfil selecionado (ex: "Administrador")

        // Usado para preencher o Dropdown na View
        public List<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();
    }
}