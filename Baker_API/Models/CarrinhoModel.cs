namespace Baker_API.Models
{
    public class CarrinhoModel
    {
        public Guid? CD_USUARIO { get; set; }
        public int CD_PRODUTO { get; set; }
        public int QT_PRODUTO { get; set; }
        public decimal VL_PRECO { get; set; }

    }
}
