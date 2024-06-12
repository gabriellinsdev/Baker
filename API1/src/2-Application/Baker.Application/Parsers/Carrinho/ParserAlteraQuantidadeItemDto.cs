using Baker.Application.Dtos.Carrinho;
using Baker.Domain.Entities;

namespace Baker.Application.Parsers.Carrinho
{
    public static class ParserAlteraQuantidadeItemDto
    {
        public static async Task<ItemCarrinho> Parse(AlterarQuantidadeItemDto item, ItemCarrinho retorno)
        {
            return await Task.FromResult(new ItemCarrinho()
            {
                CdItemDoCarrinho = item.CodigoItemDoCarrinho,
                CdCarrinho = retorno.CdCarrinho,
                CdProduto = retorno.CdProduto,
                QtProduto = item.Quantidade,
                VlPreco = retorno.VlPreco
            });
        }
    }
}
