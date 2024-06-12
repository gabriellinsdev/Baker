using System.ComponentModel.DataAnnotations.Schema;

namespace Baker.Domain.Entities
{
    [Table("TBL_PEDIDOS")]
    public class Pedido
    {
        public Guid CdPedido { get; set; }

        public Guid CdPadeiro { get; set; }

        public Guid CdCliente { get; set; }

        public DateTime DtPedido { get; set; }

        public decimal VlTotal { get; set; }

        public string NmEstado { get; set; } = null!;

        public string NmCidade { get; set; } = null!;

        public string DsEndereco { get; set; } = null!;

        public string CdCep { get; set; } = null!;

        public string? DsObservacao { get; set; }

        public virtual Usuario Padeiro { get; set; } = null!;

        public virtual Usuario Cliente { get; set; } = null!;

        public virtual ICollection<ItemPedido> ItensPedido { get; set; } = null!;
    }
}
