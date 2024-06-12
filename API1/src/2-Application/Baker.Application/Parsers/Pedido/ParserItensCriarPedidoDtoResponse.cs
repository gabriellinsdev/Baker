using Baker.Application.Dtos.Pedido;
using Baker.Domain.Entities;

namespace Baker.Application.Parsers.Pedido
{
    public static class ParserItensCriarPedidoDtoResponse
    {
        public static async Task<ItemPedidoDto> Parse(ItemCarrinho item, string nmProduto)
        {
            return await Task.FromResult(new ItemPedidoDto()
            {
                NomeProduto = nmProduto,
                QuantidadeProduto = item.QtProduto,
                Valor = item.VlPreco,
            });
        }

        public static async Task<ItemPedidoDto> Parse(ItemPedido item, string nmProduto)
        {
            return await Task.FromResult(new ItemPedidoDto()
            {
                NomeProduto = nmProduto,
                QuantidadeProduto = item.QtProduto,
                Valor = item.VlPreco,
            });
        }
    }
}
