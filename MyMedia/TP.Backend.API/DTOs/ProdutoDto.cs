namespace TP.Shared.RCL.DTOs
{
    public class ProdutoDto
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Stock { get; set; } // <--- Novo
        public string ImagemUrl { get; set; } = string.Empty;

        public string CategoriaNome { get; set; } = string.Empty;
        public string ModoEntregaNome { get; set; } = string.Empty;

        // --- Campos para Criação/Edição ---
        public int CategoriaId { get; set; } // <--- Novo
        public int ModoEntregaId { get; set; } // <--- Novo
    }
}