using Baker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baker.Infra.Data.Context.Configuration
{
    public class ConfigurationItensCarrinho : IEntityTypeConfiguration<ItemCarrinho>
    {
        public void Configure(EntityTypeBuilder<ItemCarrinho> builder)
        {
            builder.HasKey(x => x.CdItemDoCarrinho);

            builder
                .HasOne(p => p.Carrinho)
                .WithMany(t => t.ItensCarrinho)
                .HasForeignKey(c => c.CdCarrinho);

            builder
                .Property(nameof(ItemCarrinho.CdItemDoCarrinho))
                .IsRequired()
                .HasColumnName("CD_ITENS_DO_CARRINHO")
                .HasColumnType("INT")
                .UseIdentityColumn(1, 1);


            builder
                .Property(nameof(ItemCarrinho.CdCarrinho))
                .IsRequired()
                .HasColumnName("CD_CARRINHO")
                .HasColumnType("INT");

            builder
                .Property(nameof(ItemCarrinho.CdProduto))
                .IsRequired()
                .HasColumnName("CD_PRODUTO")
                .HasColumnType("INT ");

            builder
                .Property(nameof(ItemPedido.QtProduto))
                .IsRequired()
                .HasColumnName("QT_PRODUTO")
                .HasColumnType("SMALLINT");

            builder
                .Property(nameof(ItemCarrinho.VlPreco))
                .IsRequired()
                .HasColumnName("VL_PRECO")
                .HasColumnType("DECIMAL(6,2)");
        }
    }
}
