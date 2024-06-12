using Baker.Application.Dtos.Pedido;

namespace Baker.Application.Parsers.Pedido
{
    public static class ParserGetPedidosDto
    {
        public static async Task<GetPedidosDto> Parse(Domain.Entities.Pedido item, Domain.Entities.Usuario padeiro, Domain.Entities.Usuario cliente, IEnumerable<ItemPedidoDto> itens)
        {
            return await Task.FromResult(new GetPedidosDto()
            {
                CodigoPedido = item.CdPedido,
                NomePadeiro = padeiro.NmUsuario,
                NomeCliente = cliente.NmUsuario,
                Data = item.DtPedido,
                Valor = item.VlTotal,
                Estado = item.NmEstado,
                Cidade = item.NmCidade,
                Endereco = item.DsEndereco,
                Cep = item.CdCep,
                Observacao = item.DsObservacao,
                Itens = itens
            });
        }
    }
}
