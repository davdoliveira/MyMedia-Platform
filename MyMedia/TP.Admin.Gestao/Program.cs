using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TP.Admin.Gestao.Components;
using TP.Backend.Data;
using TP.Backend.Data.Entidades; // Verifica se o namespace das tuas entidades é este

var builder = WebApplication.CreateBuilder(args);

// ==============================================================================
// 1. CONFIGURAÇÃO DA BASE DE DADOS E IDENTITY
// ==============================================================================

// Ler a connection string do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MyMediaConnection")
    ?? throw new InvalidOperationException("Connection string 'MyMediaConnection' not found.");

// Registar o Contexto da Base de Dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registar o Identity (Gestão de Utilizadores/Logins)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configurações de Password para facilitar o desenvolvimento (menos restritivo)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Serviços de Autenticação do Blazor (Necessário para o <AuthorizeView> funcionar)
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// ==============================================================================

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Ativar Autenticação e Autorização no Pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();