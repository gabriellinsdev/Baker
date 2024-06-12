namespace Baker.Application.Dtos.Pedido
{
    public class CriarPedidoDtoRequest
    {
        public Guid CodigoCliente { get; set; }

        public string Estado { get; set; } = null!;

        public string Cidade { get; set; } = null!;

        public string Endereco { get; set; } = null!;

        public string Cep { get; set; } = null!;

        public string? Observacao { get; set; }

        public List<int> CodigoItemDoCarrinho { get; set; } = null!;
    }
}
