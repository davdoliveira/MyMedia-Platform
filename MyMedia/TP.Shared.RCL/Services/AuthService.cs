using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient,
                           ILocalStorageService localStorage,
                           AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDto loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (!result.IsSuccessStatusCode)
                return false;

            var response = await result.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (response != null)
            {
                // 1. Guardar Token no LocalStorage
                await _localStorage.SetItemAsync("authToken", response.Token);
                await _localStorage.SetItemAsync("userName", response.User);

                // 2. Avisar o Blazor que o utilizador mudou (Notificar o Provider)
                ((CustomAuthStateProvider)_authStateProvider).MarkUserAsAuthenticated(response.Token);

                // 3. Configurar o Header para futuros pedidos (ex: Encomendas)
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", response.Token);

                return true;
            }
            return false;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
            return result.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userName");

            ((CustomAuthStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}