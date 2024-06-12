using Baker.Application.Dtos.Carrinho;
using Baker.Application.Dtos.Pedido;
using Baker.Application.Interfaces;
using Baker.Application.Parsers.Carrinho;
using Baker.Application.Parsers.Pedido;
using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Services;
using System.Collections.Concurrent;

namespace Baker.Application.Services
{
    public class ItemCarrinhoAppService : IItemCarrinhoAppService
    {
        private readonly IItemCarrinhoService _itemCarrinhoService;
        private readonly ICarrinhoService _carrinhoService;
        private readonly IProdutoService _produtoService;

        public ItemCarrinhoAppService(IItemCarrinhoService itemCarrinhoService, ICarrinhoService carrinhoService, IProdutoService produtoService)
        {
            _itemCarrinhoService = itemCarrinhoService;
            _carrinhoService = carrinhoService;
            _produtoService = produtoService;
        }

        public async Task DeletaItem(int id)
        {
            ItemCarrinho item = await _itemCarrinhoService.GetItemCarrinhoById(id);
            await _itemCarrinhoService.DeletaItem(item);
        }

        public async Task InsereItem(InserirItemDto item)
        {
            int carrinhoId;
            Carrinho carrinho = await _carrinhoService.GetCarrinhoByUsuarioId(item.CodigoUsuario);

            if (carrinho is null) carrinhoId = await _carrinhoService.CriaCarrinho(await ParserCriaCarrinhoDto.Parse(item.CodigoUsuario));
            else carrinhoId = carrinho.CdCarrinho;

            await _itemCarrinhoService.InsereItem(await ParserInserirItemCarrinhoDto.Parse(item, carrinhoId));
        }

        public async Task AlteraQuantidadeItem(AlterarQuantidadeItemDto item)
        {
            ItemCarrinho retorno = await _itemCarrinhoService.GetItemCarrinhoById(item.CodigoItemDoCarrinho);
            await _itemCarrinhoService.AtualizaQuantidadeItem(await ParserAlteraQuantidadeItemDto.Parse(item, retorno));
        }

        public async Task<IEnumerable<GetItemCarrinhoDto>> GetItensCarrinho(Guid usuarioId) 
        {
            Carrinho carrinho = await _carrinhoService.GetCarrinhoByUsuarioId(usuarioId);
            IEnumerable<ItemCarrinho> itens =  await _itemCarrinhoService.GetItemCarrinhoByCarrinhoId(carrinho.CdCarrinho);

            ConcurrentBag<GetItemCarrinhoDto> itemsRetorno = new();

            foreach (var item in itens)
            {
                Produto produto = await _produtoService.GetProdutoById(item.CdProduto);
                itemsRetorno.Add(await ParserGetItemCarrinhoDto.Parse(item, produto.NmProduto));
            }
            return itemsRetorno;
        }
    }
}
