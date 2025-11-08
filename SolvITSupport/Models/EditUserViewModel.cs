using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // <-- Adicione esta linha no topo do ficheiro
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SolvITSupport.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        public string Cargo { get; set; }

        public string Departamento { get; set; }

        // Lista de papéis que o utilizador tem
        public IList<string> UserRoles { get; set; }

        [ValidateNever] 
        public List<SelectListItem> AllRoles { get; set; }

        public string StatusMessage { get; set; }
    }
}