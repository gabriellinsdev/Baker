namespace Baker.Application.Dtos.Pedido
{
    public class ItemPedidoDto
    {
        public string NomeProduto { get; set; } = null!;

        public short QuantidadeProduto { get; set; }

        public decimal Valor { get; set; }
    }
}
