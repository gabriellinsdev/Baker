using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;

namespace Baker.Domain.Services
{
    public class ItemPedidoService : IItemPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemPedidoRepository _itemPedidoRepository;

        public ItemPedidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _itemPedidoRepository = _unitOfWork.ItemPedidoRepository;
        }

        public async Task<IEnumerable<ItemPedido>> GetItemPedidoByPedidoId(Guid id)
        {
            return await _itemPedidoRepository.GetAllById(x => x.CdPedido == id);
        }

        public async Task InsereItens(IEnumerable<ItemPedido> itens)
        {
            await _itemPedidoRepository.Insert(itens);
            await _unitOfWork.Save();
        }
    }
}
