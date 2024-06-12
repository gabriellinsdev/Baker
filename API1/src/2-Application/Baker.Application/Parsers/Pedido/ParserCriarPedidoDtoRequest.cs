using Baker.Application.Dtos.Pedido;
using Baker.Domain.Entities;

namespace Baker.Application.Parsers.Pedido
{
    public static class ParserCriarPedidoRequestDto
    {
        public static async Task<Domain.Entities.Pedido> Parse(CriarPedidoDtoRequest item, IEnumerable<ItemCarrinho> itens, Produto produto)
        {
            return await Task.FromResult(new Domain.Entities.Pedido()
            {
                CdPedido = Guid.NewGuid(),
                CdPadeiro = produto.CdPadeiro,
                CdCliente = item.CodigoCliente,
                DtPedido = DateTime.Now,
                VlTotal = itens.Sum(x => x.QtProduto * x.VlPreco),
                NmEstado = item.Estado,
                NmCidade = item.Cidade,
                DsEndereco = item.Endereco,
                CdCep = item.Cep,
                DsObservacao = item.Observacao
            });
        }
    }
}
