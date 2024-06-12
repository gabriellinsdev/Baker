namespace Baker.Application.Dtos.Carrinho
{
    public class GetItemCarrinhoDto
    {
        public int CodigoItemCarrinho { get; set; }

        public int CodigoProduto { get; set; }

        public string NomeProduto { get; set; } = null!;

        public short Quantidade { get; set; }

        public decimal Valor { get; set; }
    }
}
