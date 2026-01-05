using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP.Backend.Data;
using TP.Backend.Data.Entidades;
using TP.Shared.RCL.DTOs;

namespace TP.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/categorias
        // CORREÇÃO: Agora devolve TODAS as categorias numa lista simples.
        // O Frontend é que decide como organizá-las (Pai -> Filho).
        [HttpGet]
        public async Task<ActionResult<List<CategoriaDto>>> GetCategorias()
        {
            var todas = await _context.Categorias.ToListAsync();

            var dtos = todas.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao ?? "",
                CategoriaPaiId = c.CategoriaPaiId // Essencial para o filtro funcionar
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/categorias/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
        {
            var c = await _context.Categorias.FindAsync(id);
            if (c == null) return NotFound();

            return new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                CategoriaPaiId = c.CategoriaPaiId
            };
        }
    }
}