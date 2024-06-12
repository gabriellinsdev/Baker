namespace Baker.Application.Dtos.Pedido
{
    public class GetTopProdutosDto
    {
        public Guid NomePadeiro { get; set; }

        public int NomeProduto { get; set; }

        public int QuantidadeVendida { get; set; }
    }
}