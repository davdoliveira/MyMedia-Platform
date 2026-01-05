using Microsoft.AspNetCore.Authentication.JwtBearer; // <--- NOVO
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // <--- NOVO
using System.Text; // <--- NOVO
using TP.Backend.Data;
using TP.Backend.Data.Entidades;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contentor.

builder.Services.AddControllers();
// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- INICIO DA CONFIGURAÇÃO DA BD E IDENTITY ---

// 1. Configurar a Ligação SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Atenção: Certifique-se que a classe ApplicationDbContext existe no projeto TP.Backend.Data
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Usa o caminho completo para garantir que é o da Microsoft
builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ==========================================================
// 2. CONFIGURAÇÃO JWT
// ==========================================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key em falta!");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    // Define que o esquema padrão é Bearer (Token)
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});
// ==========================================================
// --- FIM DA CONFIGURAÇÃO ---

var app = builder.Build();

// ==========================================
// BLOCO DE SEED DATA (Adiciona isto)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Chama o método estático que criámos
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao popular a base de dados.");
    }
}

// Configurar o pipeline de pedidos HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // <--- Verifica "Quem és tu?" (Lê o Token)
app.UseAuthorization();  // <--- Verifica "O que podes fazer?"

app.MapControllers();

app.Run();