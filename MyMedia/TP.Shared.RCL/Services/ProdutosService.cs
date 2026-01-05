using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public class ProdutosService : IProdutosService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        
        public ProdutosService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ProdutoDto>> GetProdutosAsync(int? categoriaId = null)
        {
            try
            {
                string url = "api/produtos";

                // Se houver ID, adiciona à URL (ex: api/produtos?categoriaId=5)
                if (categoriaId.HasValue)
                {
                    url += $"?categoriaId={categoriaId}";
                }

                var lista = await _httpClient.GetFromJsonAsync<List<ProdutoDto>>(url);
                return lista ?? new List<ProdutoDto>();
            }
            catch { return new List<ProdutoDto>(); }
        }

        public async Task<ProdutoDto?> GetProdutoByIdAsync(int id)
        {
            try { return await _httpClient.GetFromJsonAsync<ProdutoDto>($"api/produtos/{id}"); }
            catch { return null; }
        }

        public async Task<List<ProdutoDto>> GetMeusProdutosAsync()
        {
            // 1. Adicionar Token
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                return await _httpClient.GetFromJsonAsync<List<ProdutoDto>>("api/produtos/meus")
                        ?? new List<ProdutoDto>();
            }
            catch
            {
                return new List<ProdutoDto>();
            }
        }

        public async Task<bool> CriarProdutoAsync(ProdutoDto produto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PostAsJsonAsync("api/produtos", produto);
            return result.IsSuccessStatusCode;
        }

        // --- NOVO MÉTODO: APROVAR PRODUTO ---
        public async Task<bool> AprovarProdutoAsync(int id, decimal margem)
        {
            // Adicionar Token
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // --- CORREÇÃO MANUAL SEM O PACOTE JSON ---

            // 1. Serializar o valor (margem) para JSON manualmente
            var conteudoJson = System.Text.Json.JsonSerializer.Serialize(margem);

            // 2. Criar o conteúdo HTTP
            var content = new StringContent(conteudoJson, System.Text.Encoding.UTF8, "application/json");

            // 3. Usar o PutAsync normal (que o HttpClient tem sempre)
            var response = await _httpClient.PutAsync($"api/produtos/{id}/aprovar", content);

            return response.IsSuccessStatusCode;
        }
        // Adiciona este método dentro da classe ProdutosService
        public async Task<List<ModoEntregaDto>> GetModosEntregaAsync()
        {
            try
            {
                // Assume que o endpoint é "api/modosentrega" (confirma no teu Controller)
                return await _httpClient.GetFromJsonAsync<List<ModoEntregaDto>>("api/modosentrega")
                       ?? new List<ModoEntregaDto>();
            }
            catch
            {
                return new List<ModoEntregaDto>();
            }
        }
        public async Task<ProdutoDto?> GetProdutoDestaqueAsync()
        {
            try
            {
                // Chama o endpoint /api/produtos/destaque
                return await _httpClient.GetFromJsonAsync<ProdutoDto>("api/produtos/destaque");
            }
            catch
            {
                // Se der erro ou não houver produtos, devolve null (não mostra destaque)
                return null;
            }
        }

        // Adiciona este método dentro da classe ProdutosService
        public async Task<bool> AtualizarProdutoAsync(int id, ProdutoDto produto)
        {
            // Adicionar Token de Autenticação (Necessário para segurança)
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Envia o pedido PUT para a API
            // Nota: Certifica-te que tens: using System.Net.Http.Json; no topo do ficheiro
            var response = await _httpClient.PutAsJsonAsync($"api/produtos/{id}", produto);

            return response.IsSuccessStatusCode;
        }
    }
}