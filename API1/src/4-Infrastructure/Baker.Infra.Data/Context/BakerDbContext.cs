using Baker.Domain.Entities;
using Baker.Infra.Data.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Baker.Infra.Data.Context
{
    public class BakerDbContext : DbContext
    {
        public BakerDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigurationUsuario());
            modelBuilder.ApplyConfiguration(new ConfigurationProduto());
            modelBuilder.ApplyConfiguration(new ConfigurationPedido());
            modelBuilder.ApplyConfiguration(new ConfigurationCarrinho());
            modelBuilder.ApplyConfiguration(new ConfigurationItensPedido());
            modelBuilder.ApplyConfiguration(new ConfigurationItensCarrinho());
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        public virtual DbSet<Produto> Produtos { get; set; }

        public virtual DbSet<Pedido> Pedidos { get; set; }

        public virtual DbSet<ItemPedido> ItensPedido { get; set; }

        public virtual DbSet<Carrinho> Carrinhos { get; set; }

        public virtual DbSet<ItemCarrinho> ItensCarrinho { get; set; }
    }
}
