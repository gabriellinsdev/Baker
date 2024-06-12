namespace Baker.Application.Dtos.Carrinho
{
    public class InserirItemDto
    {
        public Guid CodigoUsuario { get; set; }

        public int CodigoProduto { get; set; }

        public short Quantidade { get; set; }

        public decimal Valor { get; set; }
    }
}
