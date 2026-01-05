using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public interface ICategoriasService
    {
        // Vai buscar a árvore completa (Pai -> Filhos)
        Task<List<CategoriaDto>> GetCategoriasAsync();

        // Vai buscar uma lista simples (útil para dropdowns no futuro)
        Task<List<CategoriaDto>> GetCategoriasFlatAsync();
    }
}