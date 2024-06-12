using System.ComponentModel.DataAnnotations.Schema;

namespace Baker.Domain.Entities
{
    [Table("TBL_ITENS_DO_CARRINHO")]
    public class ItemCarrinho
    {
        public int CdItemDoCarrinho { get; set; }

        public int CdCarrinho { get; set; }

        public int CdProduto { get; set; }

        public short QtProduto { get; set; }

        public decimal VlPreco { get; set; }

        public virtual Carrinho Carrinho { get; set; } = null!;
    }
}
