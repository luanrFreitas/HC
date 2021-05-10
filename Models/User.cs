using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Models
{
    public class User
    {
        public int ID { get; set; }
        [Display(Name ="Usuário")]
        [Required(ErrorMessage ="O usuário é obrigatório")]
        [RegularExpression(@"^[a-zA-Z0-9 ./_()-]+$", ErrorMessage = "O nome de usuário não pode ter caracteres especiais")]
        [MaxLength(12,ErrorMessage ="O Tamanho máximo são 12 caracteres")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [Required(ErrorMessage ="A senha é obrigatória")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "O senha e a confirmação de senha não são iguais")]
        public string ConfirmPassword { get; set; }

    }
}
