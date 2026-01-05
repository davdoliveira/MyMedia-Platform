using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace TP.Backend.Data.Entidades
{
    public class ModoEntrega
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty; // Ex: "Digital", "CTT"

        public string? Descricao { get; set; }

        // Um modo de entrega pode estar em vários produtos
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}