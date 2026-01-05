using System.Net.Http.Json;
using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public class CategoriasService : ICategoriasService
    {
        private readonly HttpClient _http;

        public CategoriasService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoriaDto>> GetCategoriasAsync()
        {
            try
            {
                // Chama o endpoint principal que criámos na API
                return await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias")
                       ?? new List<CategoriaDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar categorias: {ex.Message}");
                return new List<CategoriaDto>();
            }
        }

        public async Task<List<CategoriaDto>> GetCategoriasFlatAsync()
        {
            try
            {
                // Chama o endpoint "flat" (lista simples)
                return await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias/flat")
                       ?? new List<CategoriaDto>();
            }
            catch
            {
                return new List<CategoriaDto>();
            }
        }
    }
}