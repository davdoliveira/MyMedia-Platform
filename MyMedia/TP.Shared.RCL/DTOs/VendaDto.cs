using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Shared.RCL.DTOs
{
    public class VendaDto
    {
        public int EncomendaId { get; set; }
        public DateTime Data { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;
        public string EstadoEncomenda { get; set; } = string.Empty;
    }
}
