using Baker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baker.Infra.Data.Context.Configuration
{
    public class ConfigurationCarrinho : IEntityTypeConfiguration<Carrinho>
    {
        public void Configure(EntityTypeBuilder<Carrinho> builder)
        {
            builder.HasKey(x => x.CdCarrinho);

            builder
                .HasOne(p => p.Cliente)
                .WithOne(t => t.Carrinho)
                .HasForeignKey<Carrinho>(c => c.CdCliente);

            builder
                .Property(nameof(Carrinho.CdCarrinho))
                .IsRequired()
                .HasColumnName("CD_CARRINHO")
                .HasColumnType("INT")
                .UseIdentityColumn(1, 1);

            builder
                .Property(nameof(Carrinho.CdCliente))
                .IsRequired()
                .HasColumnName("CD_USUARIO")
                .HasColumnType("UNIQUEIDENTIFIER");
        }
    }
}
