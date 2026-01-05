using System.ComponentModel.DataAnnotations;
using TP.Backend.Data.Entidades; // ou .Entities, confirma o teu namespace

namespace TP.Backend.API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Opcional: Para definir se é Cliente ou Fornecedor no registo
        public UserType Tipo { get; set; }
    }
}
