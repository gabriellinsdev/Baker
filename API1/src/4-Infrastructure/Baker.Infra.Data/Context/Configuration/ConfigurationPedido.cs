using Baker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baker.Infra.Data.Context.Configuration
{
    public class ConfigurationPedido : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(x => x.CdPedido);

            builder
                .HasOne(p => p.Padeiro)
                .WithMany(t => t.PedidosPadeiro)
                .HasForeignKey(c => c.CdPadeiro);

            builder
                .HasOne(p => p.Cliente)
                .WithMany(t => t.PedidosCliente)
                .HasForeignKey(c => c.CdCliente);

            builder
                .Property(nameof(Pedido.CdPedido))
                .IsRequired()
                .HasColumnName("CD_PEDIDO")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder
                .Property(nameof(Pedido.CdPadeiro))
                .IsRequired()
                .HasColumnName("CD_PADEIRO")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder
                .Property(nameof(Pedido.CdCliente))
                .IsRequired()
                .HasColumnName("CD_CLIENTE")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder
                .Property(nameof(Pedido.DtPedido))
                .IsRequired()
                .HasColumnName("DT_PEDIDO")
                .HasColumnType("DATETIME");

            builder
                .Property(nameof(Pedido.VlTotal))
                .IsRequired()
                .HasColumnName("VL_TOTAL")
                .HasColumnType("DECIMAL(8,2)");

            builder
                .Property(nameof(Pedido.NmEstado))
                .IsRequired()
                .HasColumnName("NM_ESTADO")
                .HasColumnType("VARCHAR(50)");

            builder
                .Property(nameof(Pedido.NmCidade))
                .IsRequired()
                .HasColumnName("NM_CIDADE")
                .HasColumnType("VARCHAR(50)");

            builder
                .Property(nameof(Pedido.DsEndereco))
                .IsRequired()
                .HasColumnName("DS_ENDERECO")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(nameof(Pedido.CdCep))
                .IsRequired()
                .HasColumnName("CD_CEP")
                .HasColumnType("CHAR(8)");

            builder
                .Property(nameof(Pedido.DsObservacao))
                .IsRequired(false)
                .HasColumnName("DS_OBSERVACAO")
                .HasColumnType("VARCHAR(300)");
        }
    }
}
