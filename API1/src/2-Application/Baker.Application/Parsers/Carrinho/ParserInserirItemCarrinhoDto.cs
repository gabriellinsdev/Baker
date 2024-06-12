using Baker.Application.Dtos.Carrinho;
using Baker.Domain.Entities;

namespace Baker.Application.Parsers.Carrinho
{
    public static class ParserInserirItemCarrinhoDto
    {
        public static async Task<ItemCarrinho> Parse(InserirItemDto item, int carrinhoId)
        {
            return await Task.FromResult(new ItemCarrinho()
            {
                CdCarrinho = carrinhoId,
                CdProduto = item.CodigoProduto,
                QtProduto = item.Quantidade,
                VlPreco = item.Valor,
            });
        }
    }
}
