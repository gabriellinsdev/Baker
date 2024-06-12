using Baker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baker.Infra.Data.Context.Configuration
{
    public class ConfigurationItensPedido : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.HasKey(x => x.CdItemDoPedido);

            builder
                .HasOne(p => p.Pedido)
                .WithMany(t => t.ItensPedido)
                .HasForeignKey(c => c.CdPedido);

            builder
                .Property(nameof(ItemPedido.CdItemDoPedido))
                .IsRequired()
                .HasColumnName("CD_ITENS_DO_PEDIDO")
                .HasColumnType("INT")
                .UseIdentityColumn(1, 1);

            builder
                .Property(nameof(ItemPedido.CdPedido))
                .IsRequired()
                .HasColumnName("CD_PEDIDO")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder
                .Property(nameof(ItemPedido.CdProduto))
                .IsRequired()
                .HasColumnName("CD_PRODUTO")
                .HasColumnType("INT ");

            builder
                .Property(nameof(ItemPedido.QtProduto))
                .IsRequired()
                .HasColumnName("QT_PRODUTO")
                .HasColumnType("SMALLINT");

            builder
                .Property(nameof(ItemPedido.VlPreco))
                .IsRequired()
                .HasColumnName("VL_PRECO")
                .HasColumnType("DECIMAL(6,2)");
        }
    }
}
