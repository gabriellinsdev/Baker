using Baker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baker.Infra.Data.Context.Configuration
{
    public class ConfigurationUsuario : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.CdUsuario);

            builder
                .Property(nameof(Usuario.CdUsuario))
                .IsRequired()
                .HasColumnName("CD_USUARIO")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder
                .Property(nameof(Usuario.NmUsuario))
                .IsRequired()
                .HasColumnName("NM_USUARIO")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(nameof(Usuario.DsEmail))
                .IsRequired()
                .HasColumnName("DS_EMAIL")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(nameof(Usuario.DsTelefone))
                .IsRequired()
                .HasColumnName("DS_TELEFONE")
                .HasColumnType("VARCHAR(11)");

            builder
                .Property(nameof(Usuario.NmEstado))
                .IsRequired()
                .HasColumnName("NM_ESTADO")
                .HasColumnType("VARCHAR(50)");

            builder
                .Property(nameof(Usuario.NmCidade))
                .IsRequired()
                .HasColumnName("NM_CIDADE")
                .HasColumnType("VARCHAR(50)");

            builder
                .Property(nameof(Usuario.DsEndereco))
                .IsRequired()
                .HasColumnName("DS_ENDERECO")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(nameof(Usuario.CdCep))
                .IsRequired()
                .HasColumnName("CD_CEP")
                .HasColumnType("CHAR(8)");

            builder
                .Property(nameof(Usuario.CdSenha))
                .IsRequired()
                .HasColumnName("CD_SENHA")
                .HasColumnType("VARCHAR(60)");

            builder
                .Property(nameof(Usuario.CdCpfCnpj))
                .IsRequired()
                .HasColumnName("CD_CPF_CNPJ")
                .HasColumnType("VARCHAR(14)");
        }
    }
}
