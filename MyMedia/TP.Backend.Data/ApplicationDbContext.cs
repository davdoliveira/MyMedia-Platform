using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP.Backend.Data.Entidades; // Garante que este using está correto

namespace TP.Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabelas da BD
        // Adiciona " = null!;" no fim de cada linha
        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Encomenda> Encomendas { get; set; } = null!;
        public DbSet<ItemEncomenda> ItensEncomenda { get; set; } = null!;
        public DbSet<ModoEntrega> ModosEntrega { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // OBRIGATÓRIO para o Identity

            // --- CONFIGURAÇÕES ---

            // Configurar ItemEncomenda para NÃO apagar em cascata se o Produto for apagado
            // (Isto evita perderes o histórico de vendas se apagares um produto do catálogo)
            builder.Entity<ItemEncomenda>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // DICA EXTRA: Configurar precisão dos decimais para evitar avisos no SQL
            builder.Entity<Produto>()
                .Property(p => p.PrecoBase).HasColumnType("decimal(18,2)");
            builder.Entity<Produto>()
                .Property(p => p.PrecoFinal).HasColumnType("decimal(18,2)");
            builder.Entity<ItemEncomenda>()
                .Property(i => i.PrecoUnitario).HasColumnType("decimal(18,2)");
            builder.Entity<Encomenda>()
                .Property(e => e.Total).HasColumnType("decimal(18,2)");
        }
    }
}