using TP.Frontend.Components;
using TP.Shared.RCL.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

// 1. PRIMEIRO crias o builder
var builder = WebApplication.CreateBuilder(args);

// 2. DEPOIS adicionas os serviços ao builder
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- Serviços Partilhados (RCL) ---
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
// Adiciona isto junto aos outros AddHttpClient

// --- Configuração da API ---
// URL da tua API (Confirma se a porta 7140 está correta no launchSettings.json da API)
string apiUrl = "https://localhost:7140";

// Registar HttpClient para Produtos
builder.Services.AddHttpClient<IProdutosService, ProdutosService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

// Registar HttpClient para Autenticação
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<IEncomendasService, EncomendasService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<ICategoriasService, CategoriasService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
// 3. CONSTRÓIS a aplicação
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();