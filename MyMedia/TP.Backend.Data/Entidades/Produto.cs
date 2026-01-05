using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Backend.Data.Entidades
{
    public enum ProdutoEstado { Pendente, Ativo, Inativo }

    public class Produto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? ImagemUrl { get; set; }

        // Define o tipo decimal na base de dados para preços
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoFinal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MargemLucro { get; set; } // Percentagem guardada (opcional, mas ajuda)
        public ProdutoEstado Estado { get; set; } = ProdutoEstado.Pendente;
        public int Stock { get; set; }
        public bool IsParaVenda { get; set; }

        // Chaves estrangeiras
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        // --- NOVO: Relação com Modo de Entrega ---
        public int ModoEntregaId { get; set; }
        public ModoEntrega ModoEntrega { get; set; } = null!; //
        public string FornecedorId { get; set; } = string.Empty;
        public ApplicationUser Fornecedor { get; set; } = null!;

    }
}
