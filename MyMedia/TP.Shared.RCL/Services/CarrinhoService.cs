using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public class CarrinhoService : ICarrinhoService
    {
        // Lista privada, exposta apenas como leitura
        private List<ProdutoDto> _itens = new List<ProdutoDto>();

        public event Action? OnChange;

        public List<ProdutoDto> Itens => _itens;

        public decimal Total => _itens.Sum(i => i.Preco);

        public void AdicionarItem(ProdutoDto produto, int quantidade)
        {
            // Simples: adiciona X vezes o produto (podias agrupar, mas assim é mais fácil para já)
            for (int i = 0; i < quantidade; i++)
            {
                _itens.Add(produto);
            }
            NotifyStateChanged();
        }

        public void RemoverItem(ProdutoDto produto)
        {
            _itens.Remove(produto);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}