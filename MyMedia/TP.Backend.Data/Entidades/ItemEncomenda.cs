using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Backend.Data.Entidades
{
    public class ItemEncomenda
    {
        public int Id { get; set; }

        // Ligação à Encomenda (Pai)
        public int EncomendaId { get; set; }
        public Encomenda Encomenda { get; set; } = null!;

        // Ligação ao Produto
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        // REQUISITOS OBRIGATÓRIOS DO ENUNCIADO:
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; } // Para garantir o histórico correto 
    }
}