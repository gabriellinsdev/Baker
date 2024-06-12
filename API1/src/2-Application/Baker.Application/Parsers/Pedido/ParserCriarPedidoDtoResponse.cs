using Baker.Application.Dtos.Pedido;

namespace Baker.Application.Parsers.Pedido
{
    public static class ParserCriarPedidoDtoResponse
    {
        public static async Task<CriarPedidoDtoResponse> Parse(Domain.Entities.Pedido item, IEnumerable<ItemPedidoDto> itensCarrinho, Domain.Entities.Usuario cliente, Domain.Entities.Usuario padeiro)
        {
            return await Task.FromResult(new CriarPedidoDtoResponse()
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
                Itens = itensCarrinho
            });
        }
    }
}