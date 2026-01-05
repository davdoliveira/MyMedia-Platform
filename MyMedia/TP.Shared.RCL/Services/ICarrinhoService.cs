using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public interface ICarrinhoService
    {
        // Evento para avisar o layout que o carrinho mudou (para atualizar o ícone)
        event Action OnChange;

        List<ProdutoDto> Itens { get; }
        void AdicionarItem(ProdutoDto produto, int quantidade);
        void RemoverItem(ProdutoDto produto);
        decimal Total { get; }
    }
}