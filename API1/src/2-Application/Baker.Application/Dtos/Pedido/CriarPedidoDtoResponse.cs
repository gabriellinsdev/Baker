namespace Baker.Application.Dtos.Pedido
{
    public class CriarPedidoDtoResponse
    {
        public Guid CodigoPedido { get; set; }

        public string NomePadeiro { get; set; } = null!;

        public string NomeCliente { get; set; } = null!;

        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string Estado { get; set; } = null!;

        public string Cidade { get; set; } = null!;

        public string Endereco { get; set; } = null!;

        public string Cep { get; set; } = null!;

        public string? Observacao { get; set; }

        public IEnumerable<ItemPedidoDto> Itens { get; set; } = null!;
    }
}
