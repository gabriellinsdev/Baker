using System.ComponentModel.DataAnnotations.Schema;

namespace Baker.Domain.Entities
{
    [Table("TBL_CARRINHOS")]
    public class Carrinho
    {
        public int CdCarrinho { get; set; }

        public Guid CdCliente { get; set; }

        public virtual Usuario Cliente { get; set; } = null!;

        public virtual ICollection<ItemCarrinho> ItensCarrinho { get; set; } = null!;
    }
}
