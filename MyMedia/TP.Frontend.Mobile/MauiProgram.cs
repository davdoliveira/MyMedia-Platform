using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using TP.Shared.RCL.Services;

namespace TP.Frontend.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            // Carrinho local (não usa API direta para guardar, usa LocalStorage)
            builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // ========================================================
            // 1. DEFINIR O URL BASE
            // ========================================================
            // Verifica no launchSettings.json da API qual é a porta HTTPS correta
            string baseApiUrl = "https://localhost:7140";

            // Configuração Especial para Android (localhost do PC é 10.0.2.2 no Emulador)
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                baseApiUrl = baseApiUrl.Replace("localhost", "10.0.2.2");
            }

            // Handler para ignorar erros de certificado SSL em desenvolvimento (Self-Signed)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            };

            // ========================================================
            // 2. REGISTAR SERVIÇOS DA API
            // ========================================================

            // Serviço de Produtos
            builder.Services.AddHttpClient<IProdutosService, ProdutosService>(client =>
            {
                client.BaseAddress = new Uri(baseApiUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);

            // --- AQUI ESTAVA A FALTAR O SERVIÇO DE CATEGORIAS! ---
            builder.Services.AddHttpClient<ICategoriasService, CategoriasService>(client =>
            {
                client.BaseAddress = new Uri(baseApiUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
            // -----------------------------------------------------

            // Serviço de Autenticação
            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(baseApiUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);

            // Serviço de Encomendas
            builder.Services.AddHttpClient<IEncomendasService, EncomendasService>(client =>
            {
                client.BaseAddress = new Uri(baseApiUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);

            return builder.Build();
        }
    }
}