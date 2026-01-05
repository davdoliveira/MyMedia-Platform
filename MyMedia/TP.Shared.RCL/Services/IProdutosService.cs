using TP.Shared.RCL.DTOs;
namespace TP.Shared.RCL.Services
{
    public interface IProdutosService
    {
        Task<List<ProdutoDto>> GetProdutosAsync(int? categoriaId = null);
        Task<List<ProdutoDto>> GetMeusProdutosAsync();
        Task<ProdutoDto?> GetProdutoByIdAsync(int id);
        Task<ProdutoDto?> GetProdutoDestaqueAsync();
        Task<bool> CriarProdutoAsync(ProdutoDto produto);
        Task<List<ModoEntregaDto>> GetModosEntregaAsync();
        Task<bool> AtualizarProdutoAsync(int id, ProdutoDto produto);
    }
}