using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace TP.Shared.RCL.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = "";

            try
            {
                // Tenta ler o token. Se estivermos no "Prerender", isto falha com InvalidOperationException
                token = await _localStorage.GetItemAsync<string>("authToken");
            }
            catch (InvalidOperationException)
            {
                // Se deu erro, é porque estamos a carregar a página ainda.
                // Fingimos que o utilizador é anónimo por agora.
                // Assim que o site carregar (segundos depois), este método corre de novo e já funciona.
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // ... Resto do código igual ...
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt", "unique_name", "role");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void MarkUserAsAuthenticated(string token)
        {
            // CORREÇÃO AQUI TAMBÉM
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt", "unique_name", "role");
            var user = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return Enumerable.Empty<Claim>();

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}