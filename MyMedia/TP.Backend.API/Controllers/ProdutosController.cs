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
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos([FromQuery] int? categoriaId)
        {
            var query = _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.ModoEntrega)
                .Where(p => p.Estado == ProdutoEstado.Ativo);

            if (categoriaId.HasValue)
            {
                var idsFilhas = await _context.Categorias
                    .Where(c => c.CategoriaPaiId == categoriaId)
                    .Select(c => c.Id)
                    .ToListAsync();

                var idsNetas = await _context.Categorias
                    .Where(c => c.CategoriaPaiId != null && idsFilhas.Contains(c.CategoriaPaiId.Value))
                    .Select(c => c.Id)
                    .ToListAsync();

                var idsPermitidos = new List<int> { categoriaId.Value };
                idsPermitidos.AddRange(idsFilhas);
                idsPermitidos.AddRange(idsNetas);

                query = query.Where(p => idsPermitidos.Contains(p.CategoriaId));
            }

            var produtos = await query.ToListAsync();

            var produtosDto = produtos.Select(p => new ProdutoDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Descricao = p.Descricao,
                Preco = p.PrecoFinal,
                Stock = p.Stock,
                ImagemUrl = p.ImagemUrl ?? "/img/sem-imagem.png",
                CategoriaNome = p.Categoria?.Nome ?? "Sem Categoria",
                ModoEntregaNome = p.ModoEntrega?.Nome ?? "N/A"
            }).ToList();

            return Ok(produtosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
        {
            var p = await _context.Produtos
                .Include(x => x.Categoria)
                .Include(x => x.ModoEntrega)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return NotFound("Produto não encontrado.");

            var dto = new ProdutoDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Descricao = p.Descricao,
                Preco = p.PrecoFinal,
                Stock = p.Stock,
                ImagemUrl = p.ImagemUrl,
                CategoriaNome = p.Categoria?.Nome ?? "N/A",
                ModoEntregaNome = p.ModoEntrega?.Nome ?? "N/A",
                CategoriaId = p.CategoriaId,
                ModoEntregaId = p.ModoEntregaId
            };

            return Ok(dto);
        }

        [HttpPut("{id}/aprovar")]
        public async Task<IActionResult> AprovarProduto(int id, [FromBody] decimal margemPercentagem)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            produto.PrecoFinal = produto.PrecoBase + (produto.PrecoBase * (margemPercentagem / 100));
            produto.MargemLucro = margemPercentagem;
            produto.Estado = ProdutoEstado.Ativo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("destaque")]
        public async Task<ActionResult<ProdutoDto>> GetProdutoDestaque()
        {
            var produto = await _context.Produtos
                .Where(p => p.Estado == ProdutoEstado.Ativo)
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (produto == null)
            {
                return NoContent();
            }

            var dto = new ProdutoDto
            {
                Id = produto.Id,
                Titulo = produto.Titulo,
                Descricao = produto.Descricao,
                Preco = produto.PrecoFinal,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId,
                ModoEntregaId = produto.ModoEntregaId,
                Stock = produto.Stock
            };

            return Ok(dto);
        }

        [HttpGet("meus")]
        [Authorize(Roles = "Fornecedor")]
        public async Task<ActionResult<List<ProdutoDto>>> GetMeusProdutos()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.ModoEntrega)
                .Where(p => p.FornecedorId == userId)
                .ToListAsync();

            var dtos = produtos.Select(p => new ProdutoDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Descricao = p.Descricao,
                Preco = p.PrecoBase,
                Stock = p.Stock,
                ImagemUrl = p.ImagemUrl,
                CategoriaNome = p.Categoria?.Nome ?? "N/A",
                ModoEntregaNome = p.ModoEntrega?.Nome ?? "N/A",
                CategoriaId = p.CategoriaId,
                ModoEntregaId = p.ModoEntregaId
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        [Authorize(Roles = "Fornecedor,Administrador")]
        public async Task<ActionResult<ProdutoDto>> CreateProduto(ProdutoDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilizador não identificado no Token.");

            if (dto.CategoriaId <= 0) return BadRequest("Categoria inválida (ID=0).");
            if (dto.ModoEntregaId <= 0) return BadRequest("Modo de Entrega inválido (ID=0).");

            var novoProduto = new Produto
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                PrecoBase = dto.Preco,
                PrecoFinal = dto.Preco,
                Stock = dto.Stock,
                ImagemUrl = dto.ImagemUrl,
                FornecedorId = userId,
                Estado = (userRole == "Administrador") ? ProdutoEstado.Ativo : ProdutoEstado.Pendente,
                IsParaVenda = true,
                CategoriaId = dto.CategoriaId,
                ModoEntregaId = dto.ModoEntregaId
            };

            try
            {
                _context.Produtos.Add(novoProduto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                var erroInterno = dbEx.InnerException?.Message ?? dbEx.Message;
                if (erroInterno.Contains("FK_Produtos_AspNetUsers"))
                    return BadRequest($"Erro CRÍTICO: O utilizador do Token (ID: {userId}) não existe na BD. Faz Logout e Login novamente.");
                if (erroInterno.Contains("FK_Produtos_Categorias"))
                    return BadRequest($"Erro: A Categoria selecionada (ID: {dto.CategoriaId}) não existe na BD.");
                return BadRequest($"Erro de Base de Dados: {erroInterno}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro Geral: {ex.Message}");
            }

            dto.Id = novoProduto.Id;
            return CreatedAtAction(nameof(GetProduto), new { id = novoProduto.Id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Fornecedor,Administrador")]
        public async Task<IActionResult> UpdateProduto(int id, ProdutoDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var produtoDb = await _context.Produtos.FindAsync(id);
            if (produtoDb == null) return NotFound();

            if (userRole == "Fornecedor" && produtoDb.FornecedorId != userId)
            {
                return Forbid();
            }

            produtoDb.Titulo = dto.Titulo;
            produtoDb.Descricao = dto.Descricao;
            produtoDb.Stock = dto.Stock;
            produtoDb.ImagemUrl = dto.ImagemUrl;
            produtoDb.CategoriaId = dto.CategoriaId;
            produtoDb.ModoEntregaId = dto.ModoEntregaId;

            if (produtoDb.PrecoBase != dto.Preco)
            {
                produtoDb.PrecoBase = dto.Preco;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Produtos.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }
    }
}