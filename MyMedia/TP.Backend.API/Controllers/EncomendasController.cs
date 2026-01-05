using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TP.Backend.API.DTOs;
using TP.Backend.Data;
using TP.Backend.Data.Entidades;
using TP.Shared.RCL.DTOs;

namespace TP.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <--- Só utilizadores logados podem encomendar
    public class EncomendasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EncomendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/encomendas
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] List<CarrinhoItemDto> itensCarrinho)
        {
            if (itensCarrinho == null || !itensCarrinho.Any())
                return BadRequest("O carrinho está vazio.");

            // 1. Descobrir quem é o utilizador
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // 2. Preparar a Encomenda
            var novaEncomenda = new Encomenda
            {
                ClienteId = userId,
                DataEncomenda = DateTime.UtcNow,
                EstaPaga = false, // Simulação
                Estado = "Pendente", // Começa sempre como Pendente
                Itens = new List<ItemEncomenda>()
            };

            decimal totalCalculado = 0;

            // 3. Processar Itens (Validação Apenas)
            foreach (var itemDto in itensCarrinho)
            {
                // Buscar produto à BD
                var produtoDb = await _context.Produtos.FindAsync(itemDto.ProdutoId);

                if (produtoDb == null)
                    return BadRequest($"O produto {itemDto.Titulo} (ID {itemDto.ProdutoId}) já não existe.");

                // --- VALIDAÇÃO DE STOCK ---
                // Verificamos se há stock, mas NÃO DESCONTAMOS AINDA
                if (produtoDb.Stock < itemDto.Quantidade)
                {
                    return BadRequest($"Stock insuficiente para '{produtoDb.Titulo}'. Apenas {produtoDb.Stock} unidades disponíveis.");
                }

                // Criar linha da encomenda com o preço da BD
                var linha = new ItemEncomenda
                {
                    ProdutoId = produtoDb.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produtoDb.PrecoFinal
                };

                novaEncomenda.Itens.Add(linha);
                totalCalculado += (linha.PrecoUnitario * linha.Quantidade);
            }

            novaEncomenda.Total = totalCalculado;

            // 4. Guardar Encomenda
            try
            {
                _context.Encomendas.Add(novaEncomenda);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao gravar encomenda: " + ex.Message);
            }

            return Ok(new { Message = "Encomenda registada com sucesso! Aguarda expedição.", EncomendaId = novaEncomenda.Id });
        }

        // PUT: api/encomendas/{id}/estado
        // É AQUI QUE O STOCK É DESCONTADO
        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Administrador,Funcionário")] // Só gestão pode fazer isto
        public async Task<IActionResult> AlterarEstadoEncomenda(int id, [FromBody] string novoEstado)
        {
            // 1. Carregar a encomenda e os produtos associados
            var encomenda = await _context.Encomendas
                .Include(e => e.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encomenda == null) return NotFound("Encomenda não encontrada.");

            // Evitar processar duas vezes
            if (encomenda.Estado == "Expedida" && novoEstado == "Expedida")
                return BadRequest("Esta encomenda já foi expedida anteriormente.");

            // 2. LÓGICA DE STOCK (Só se estivermos a aprovar/expedir)
            if (novoEstado == "Expedida")
            {
                foreach (var item in encomenda.Itens)
                {
                    var produto = item.Produto;

                    // Verificação de Segurança de última hora
                    if (produto.Stock < item.Quantidade)
                    {
                        return BadRequest($"Não é possível expedir! O produto '{produto.Titulo}' já não tem stock suficiente (Stock Atual: {produto.Stock}).");
                    }

                    // A. Descontar Stock
                    produto.Stock -= item.Quantidade;

                    // B. Inativar se chegar a zero
                    if (produto.Stock <= 0)
                    {
                        produto.Stock = 0; // Garantir que não fica negativo
                        produto.Estado = ProdutoEstado.Inativo; // <--- O PRODUTO DESAPARECE DO FRONTEND
                    }
                }

                // Marca como paga se for expedida
                encomenda.EstaPaga = true;
            }

            // 3. Atualizar Estado
            encomenda.Estado = novoEstado;

            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Encomenda atualizada para {novoEstado} e stocks ajustados." });
        }

        // GET: api/encomendas
        [HttpGet]
        public async Task<ActionResult<List<EncomendaDto>>> GetMinhasEncomendas()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var encomendas = await _context.Encomendas
                .Include(e => e.Itens)
                .ThenInclude(i => i.Produto)
                .Where(e => e.ClienteId == userId)
                .OrderByDescending(e => e.DataEncomenda)
                .ToListAsync();

            var listaDto = encomendas.Select(e => new EncomendaDto
            {
                Id = e.Id,
                Data = e.DataEncomenda,
                Total = e.Total,
                Estado = e.Estado, // Mostra o estado real (Pendente/Expedida)
                Itens = e.Itens.Select(i => new CarrinhoItemDto
                {
                    ProdutoId = i.ProdutoId,
                    Titulo = i.Produto.Titulo,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            }).ToList();

            return Ok(listaDto);
        }

        // GET: api/encomendas/vendas (Para Fornecedores)
        [HttpGet("vendas")]
        [Authorize(Roles = "Fornecedor")]
        public async Task<ActionResult<List<VendaDto>>> GetVendasFornecedor()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vendas = await _context.ItensEncomenda
                .Include(i => i.Produto)
                .Include(i => i.Encomenda)
                .Where(i => i.Produto.FornecedorId == userId)
                .OrderByDescending(i => i.Encomenda.DataEncomenda)
                .ToListAsync();

            var dtos = vendas.Select(v => new VendaDto
            {
                EncomendaId = v.EncomendaId,
                Data = v.Encomenda.DataEncomenda,
                ProdutoNome = v.Produto.Titulo,
                Quantidade = v.Quantidade,
                ValorUnitario = v.Produto.PrecoBase,
                EstadoEncomenda = v.Encomenda.Estado // Usa o estado de texto
            }).ToList();

            return Ok(dtos);
        }
    }
}