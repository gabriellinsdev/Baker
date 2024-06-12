using System.ComponentModel.DataAnnotations.Schema;

namespace Baker.Domain.Entities
{
    [Table("TBL_ITENS_DO_PEDIDO")]
    public class ItemPedido
    {
        public int CdItemDoPedido { get; set; }

        public Guid CdPedido { get; set; }

        public int CdProduto { get; set; }

        public short QtProduto { get; set; }

        public decimal VlPreco { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
    }
}