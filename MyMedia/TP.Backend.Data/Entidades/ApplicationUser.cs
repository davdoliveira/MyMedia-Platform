using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Backend.Data.Entidades
{
    // Enumerações para definir os tipos de utilizador e estados
    public enum UserType { Cliente, Fornecedor, Funcionario, Administrador }
    public enum UserStatus { Pendente, Ativo, Suspenso }

    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;

        // NIF é opcional (?) mas recomendado para faturação
        public string? NIF { get; set; }
        public string? Morada { get; set; }

        public UserType Tipo { get; set; }

        // O estado predefinido é Pendente, conforme regra de negócio
        public UserStatus Estado { get; set; } = UserStatus.Pendente;
    }
}
