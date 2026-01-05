using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage; // <--- Precisamos disto para ler o token
using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public class EncomendasService : IEncomendasService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        // Injetamos o LocalStorage para poder ir buscar o token guardado
        public EncomendasService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<bool> FinalizarEncomendaAsync(List<CarrinhoItemDto> itens)
        {
            try
            {
                // 1. Ir buscar o Token JWT guardado no login
                var token = await _localStorage.GetItemAsync<string>("authToken");

                // 2. Se tiver token, coloca-o no cabeçalho do pedido (A "Pulseira")
                if (!string.IsNullOrEmpty(token))
                {
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                // 3. Enviar a encomenda
                var result = await _http.PostAsJsonAsync("api/encomendas", itens);

                if (!result.IsSuccessStatusCode)
                {
                    // (Opcional) Ler o erro da API para debug
                    var erro = await result.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro API: {erro}");
                }

                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar encomenda: {ex.Message}");
                return false;
            }
        }
        public async Task<List<EncomendaDto>> GetMinhasEncomendasAsync()
        {
            // 1. Adicionar Token (Segurança)
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            // 2. Pedir dados
            try
            {
                return await _http.GetFromJsonAsync<List<EncomendaDto>>("api/encomendas")
                       ?? new List<EncomendaDto>();
            }
            catch
            {
                return new List<EncomendaDto>();
            }
        }
        public async Task<List<VendaDto>> GetVendasFornecedorAsync()
        {
            // 1. Obter o Token de Autenticação
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // 2. Adicionar o Token ao pedido (Header)
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // 3. Chamar a API
                return await _http.GetFromJsonAsync<List<VendaDto>>("api/encomendas/vendas")
                       ?? new List<VendaDto>();
            }
            catch
            {
                return new List<VendaDto>();
            }
        }
    }
}