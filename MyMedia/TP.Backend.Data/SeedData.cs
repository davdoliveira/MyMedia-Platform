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

            context.Database.EnsureCreated();

            string[] roles = { "Administrador", "Cliente", "Fornecedor", "Funcionario" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            const string demoAdminPassword = "DemoAdmin123!";

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

                var result = await userManager.CreateAsync(adminUser, demoAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }

            if (!context.ModosEntrega.Any())
            {
                context.ModosEntrega.AddRange(
                    new ModoEntrega { Nome = "Download Digital", Descricao = "Envio imediato" },
                    new ModoEntrega { Nome = "CTT Expresso", Descricao = "Entrega em 24h" },
                    new ModoEntrega { Nome = "Levantamento em Loja", Descricao = "Gratuito" }
                );
                await context.SaveChangesAsync();
            }

            var filmes = await EnsureCategoria(context, "Filmes", null);
            var musica = await EnsureCategoria(context, "Música", null);
            var livros = await EnsureCategoria(context, "Livros", null);

            var acao = await EnsureCategoria(context, "Ação", filmes.Id);
            var comedia = await EnsureCategoria(context, "Comédia", filmes.Id);
            var romance = await EnsureCategoria(context, "Romance", filmes.Id);

            var rock = await EnsureCategoria(context, "Rock", musica.Id);
            var pop = await EnsureCategoria(context, "Pop", musica.Id);
            var jazz = await EnsureCategoria(context, "Jazz", musica.Id);

            var infantis = await EnsureCategoria(context, "Infantis", livros.Id);
            var biografias = await EnsureCategoria(context, "Biografias", livros.Id);

            await EnsureCategoria(context, "Metal", rock.Id);
            await EnsureCategoria(context, "Classic Rock", rock.Id);

            await EnsureCategoria(context, "Marvel", acao.Id);
            await EnsureCategoria(context, "DC Comics", acao.Id);
        }

        private static async Task<Categoria> EnsureCategoria(ApplicationDbContext context, string nome, int? paiId)
        {
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
                await context.SaveChangesAsync();
            }
            else if (paiId.HasValue && categoria.CategoriaPaiId != paiId)
            {
                categoria.CategoriaPaiId = paiId;
                context.Categorias.Update(categoria);
                await context.SaveChangesAsync();
            }

            return categoria;
        }
    }
}