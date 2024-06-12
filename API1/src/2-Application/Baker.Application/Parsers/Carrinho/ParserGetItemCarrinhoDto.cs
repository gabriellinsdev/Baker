using Baker.Application.Dtos.Carrinho;
using Baker.Domain.Entities;

namespace Baker.Application.Parsers.Carrinho
{
    public static class ParserGetItemCarrinhoDto
    {
        public static async Task<GetItemCarrinhoDto> Parse(ItemCarrinho item, string nmProduto)
        {
            return await Task.FromResult(new GetItemCarrinhoDto()
            {
                CodigoItemCarrinho = item.CdItemDoCarrinho,
                CodigoProduto = item.CdProduto,
                NomeProduto = nmProduto,
                Quantidade = item.QtProduto,
                Valor = item.VlPreco
            });
        }
    }
}
