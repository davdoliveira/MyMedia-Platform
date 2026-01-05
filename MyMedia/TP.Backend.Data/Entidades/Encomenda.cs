using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Backend.Data.Entidades
{
    public class Encomenda
    {
        public int Id { get; set; }
        public DateTime DataEncomenda { get; set; } = DateTime.UtcNow;
        public bool EstaPaga { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public string ClienteId { get; set; } = string.Empty;
        public ApplicationUser Cliente { get; set; } = null!;

        public string Estado { get; set; } = "Pendente";

        // Alterado para usar a classe ItemEncomenda
        public ICollection<ItemEncomenda> Itens { get; set; } = new List<ItemEncomenda>();
    }
}