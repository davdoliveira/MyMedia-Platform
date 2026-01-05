using System.Collections.Generic;

namespace TP.Shared.RCL.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        // Apenas o ID do pai, para referência (evita trazer o objeto Pai inteiro)
        public int? CategoriaPaiId { get; set; }

        // A lista de filhas. Inicializamos vazia para não dar NullReferenceException.
        public List<CategoriaDto> SubCategorias { get; set; } = new();
    }
}