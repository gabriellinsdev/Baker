using Baker.Domain.Entities;
using System.Collections.Concurrent;

namespace Baker.Application.Parsers.Pedido
{
    public static class ParserInserirItemPedidoDto
    {
        public static async Task<ItemPedido> Parse(ItemCarrinho item, Domain.Entities.Pedido pedido)
        {
            return await Task.FromResult(new ItemPedido()
            {
                CdPedido = pedido.CdPedido,
                CdProduto = item.CdProduto,
                QtProduto = item.QtProduto,
                VlPreco = item.VlPreco,
            });
        }

        public static async Task<IEnumerable<ItemPedido>> Parse(IEnumerable<ItemCarrinho> items, Domain.Entities.Pedido pedido)
        {
            ConcurrentBag<ItemPedido> itemsRetorno = new();
            items.ToList().ForEach(async x => itemsRetorno.Add(await Parse(x, pedido)));
            return await Task.FromResult(itemsRetorno);
        }
    }
}
