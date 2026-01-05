using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public interface IEncomendasService
    {
        Task<bool> FinalizarEncomendaAsync(List<CarrinhoItemDto> itens);
        Task<List<EncomendaDto>> GetMinhasEncomendasAsync();
        Task<List<VendaDto>> GetVendasFornecedorAsync();
    }
}
