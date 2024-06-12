using Baker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baker.Infra.Data.Context.Configuration
{
    public class ConfigurationProduto : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.CdProduto);

            builder
                .HasOne(p => p.Padeiro)
                .WithMany(t => t.Produtos)
                .HasForeignKey(c => c.CdPadeiro);

            builder
                .Property(nameof(Produto.CdProduto))
                .IsRequired()
                .HasColumnName("CD_PRODUTO")
                .HasColumnType("INT")
                .UseIdentityColumn(1, 1);

            builder
                .Property(nameof(Produto.CdPadeiro))
                .IsRequired()
                .HasColumnName("CD_USUARIO")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder
                .Property(nameof(Produto.NmProduto))
                .IsRequired()
                .HasColumnName("NM_PRODUTO")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(nameof(Produto.DsProduto))
                .IsRequired(false)
                .HasColumnName("DS_PRODUTO")
                .HasColumnType("VARCHAR(300)");

            builder
                .Property(nameof(Produto.VlPreco))
                .IsRequired()
                .HasColumnName("VL_PRECO")
                .HasColumnType("DECIMAL(6,2)");
        }
    }
}
