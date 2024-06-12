using System.ComponentModel.DataAnnotations.Schema;

namespace Baker.Domain.Entities
{
    [Table("TBL_PRODUTOS")]
    public class Produto
    {
        public int CdProduto { get; set; }

        public Guid CdPadeiro { get; set; }

        public string NmProduto { get; set; } = null!;

        public string? DsProduto { get; set; }

        public decimal VlPreco { get; set; }

        public virtual Usuario Padeiro { get; set; } = null!;

    }
}
