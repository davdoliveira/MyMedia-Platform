using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TP.Backend.Data.Entidades;

namespace TP.Backend.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Garantir que a BD está criada
            context.Database.EnsureCreated();

            // 2. Roles
            string[] roles = { "Administrador", "Cliente", "Fornecedor", "Funcionario" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 3. Administrador
            var adminEmail = "admin@mymedia.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NomeCompleto = "Super Administrador",
                    EmailConfirmed = true,
                    NIF = "999999990",
                    Tipo = UserType.Administrador,
                    Estado = UserStatus.Ativo
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }

            // 4. Modos de Entrega
            if (!context.ModosEntrega.Any())
            {
                context.ModosEntrega.AddRange(
                    new ModoEntrega { Nome = "Download Digital", Descricao = "Envio imediato" },
                    new ModoEntrega { Nome = "CTT Expresso", Descricao = "Entrega em 24h" },
                    new ModoEntrega { Nome = "Levantamento em Loja", Descricao = "Gratuito" }
                );
                await context.SaveChangesAsync();
            }

            // 5. CATEGORIAS INTELIGENTES (Hierarquia)
            // ==========================================

            // Passo 5.1: Criar/Buscar Raízes
            var filmes = await EnsureCategoria(context, "Filmes", null);
            var musica = await EnsureCategoria(context, "Música", null);
            var livros = await EnsureCategoria(context, "Livros", null);

            // Passo 5.2: Criar Subcategorias (Ligadas aos pais acima)
            // Filmes -> ...
            var acao = await EnsureCategoria(context, "Ação", filmes.Id);
            var comedia = await EnsureCategoria(context, "Comédia", filmes.Id);
            var romance = await EnsureCategoria(context, "Romance", filmes.Id);

            // Música -> ...
            var rock = await EnsureCategoria(context, "Rock", musica.Id);
            var pop = await EnsureCategoria(context, "Pop", musica.Id);
            var jazz = await EnsureCategoria(context, "Jazz", musica.Id);

            // Livros -> ...
            var infantis = await EnsureCategoria(context, "Infantis", livros.Id);
            var biografias = await EnsureCategoria(context, "Biografias", livros.Id);

            // Passo 5.3: Sub-Subcategorias (Nível 3)
            // Música -> Rock -> ...
            await EnsureCategoria(context, "Metal", rock.Id);
            await EnsureCategoria(context, "Classic Rock", rock.Id);

            // Filmes -> Ação -> ...
            await EnsureCategoria(context, "Marvel", acao.Id);
            await EnsureCategoria(context, "DC Comics", acao.Id);
        }

        // Método auxiliar para não duplicar categorias
        private static async Task<Categoria> EnsureCategoria(ApplicationDbContext context, string nome, int? paiId)
        {
            // Tenta encontrar pelo nome
            var categoria = await context.Categorias
                .FirstOrDefaultAsync(c => c.Nome == nome);

            if (categoria == null)
            {
                categoria = new Categoria
                {
                    Nome = nome,
                    Descricao = "Categoria Automática",
                    CategoriaPaiId = paiId
                };
                context.Categorias.Add(categoria);
                await context.SaveChangesAsync(); // Grava logo para gerar o ID
            }
            else if (paiId.HasValue && categoria.CategoriaPaiId != paiId)
            {
                // Se já existia mas não tinha pai (foi criada manualmente), corrige o pai
                categoria.CategoriaPaiId = paiId;
                context.Categorias.Update(categoria);
                await context.SaveChangesAsync();
            }

            return categoria;
        }
    }
}