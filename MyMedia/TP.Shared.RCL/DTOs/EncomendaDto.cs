using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Shared.RCL.DTOs
{
    public class EncomendaDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendente";

        // Para enviar a encomenda, só precisamos de saber que produtos e quantidades
        public List<CarrinhoItemDto> Itens { get; set; } = new();
    }

    public class CarrinhoItemDto
    {
        public int ProdutoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}