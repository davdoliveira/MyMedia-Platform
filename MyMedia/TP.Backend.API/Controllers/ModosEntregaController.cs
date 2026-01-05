using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP.Backend.Data;
using TP.Backend.Data.Entidades;

namespace TP.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModosEntregaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ModosEntregaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ModoEntrega>>> GetModosEntrega()
        {
            return await _context.ModosEntrega.ToListAsync();
        }
    }
}