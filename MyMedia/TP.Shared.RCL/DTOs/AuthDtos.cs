using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace TP.Shared.RCL.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password é obrigatória")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "A password deve ter pelo menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "As passwords não coincidem")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // 0 = Cliente, 1 = Fornecedor (Podes usar um Enum aqui se preferires)
        public int Tipo { get; set; } = 0;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty; // Nome do utilizador
    }
}