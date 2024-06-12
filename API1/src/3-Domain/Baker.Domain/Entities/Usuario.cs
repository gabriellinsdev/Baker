using System.ComponentModel.DataAnnotations.Schema;

namespace Baker.Domain.Entities
{
    [Table("TBL_USUARIOS")]
    public class Usuario
    {
        public Guid CdUsuario { get; set; }

        public string NmUsuario { get; set; } = null!;

        public string DsEmail { get; set; } = null!;

        public string DsTelefone { get; set; } = null!;

        public string NmEstado { get; set; } = null!;

        public string NmCidade { get; set; } = null!;

        public string DsEndereco { get; set; } = null!;

        public string CdCep { get; set; } = null!;

        public string CdSenha { get; set; } = null!;

        public string CdCpfCnpj { get; set; } = null!;

        public virtual ICollection<Produto> Produtos { get; set; } = null!;

        public virtual Carrinho Carrinho { get; set; } = null!;

        public virtual ICollection<Pedido> PedidosCliente { get; set; } = null!;

        public virtual ICollection<Pedido> PedidosPadeiro { get; set; } = null!;

    }
}
