using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;

namespace Baker.Domain.Services
{
    public class ItemCarrinhoService : IItemCarrinhoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemCarrinhoRepository _itemCarrinhoRepository;

        public ItemCarrinhoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _itemCarrinhoRepository = _unitOfWork.ItemCarrinhoRepository;
        }

        public async Task<ItemCarrinho> GetItemCarrinhoById(int id)
        {
            return await _itemCarrinhoRepository.Get(x => x.CdItemDoCarrinho == id);
        }

        public async Task<IEnumerable<ItemCarrinho>> GetItemCarrinhoByCarrinhoId(int id)
        {
            return await _itemCarrinhoRepository.GetAllByCarrinhoId(x => x.CdCarrinho == id);
        }

        public async Task InsereItem(ItemCarrinho item)
        {
            ItemCarrinho retorno = await _itemCarrinhoRepository.Get(x => x.CdCarrinho == item.CdCarrinho && x.CdProduto == item.CdProduto);

            if (retorno is null)
            {
                await _itemCarrinhoRepository.Insert(item);
                await _unitOfWork.Save();
            }
            else
            {
                item.CdItemDoCarrinho = retorno.CdItemDoCarrinho;
                item.QtProduto += retorno.QtProduto;

                await _itemCarrinhoRepository.Update(item);
                await _unitOfWork.Save();
            }
        }

        public async Task AtualizaQuantidadeItem(ItemCarrinho item)
        {
            await _itemCarrinhoRepository.Update(item);
            await _unitOfWork.Save();
        }

        public async Task DeletaItem(ItemCarrinho item)
        {
            await _itemCarrinhoRepository.Delete(item);
            await _unitOfWork.Save();
        }

        public async Task DeletaItens(IEnumerable<ItemCarrinho> itens)
        {
            await _itemCarrinhoRepository.Delete(itens);
            await _unitOfWork.Save();
        }
    }
}
