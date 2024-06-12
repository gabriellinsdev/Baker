using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;

namespace Baker.Domain.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _pedidoRepository = _unitOfWork.PedidoRepository;
        }

        public async Task<IEnumerable<Pedido>> GetPedidosByUsuarioId(Guid id)
        {
            return await _pedidoRepository.GetAllById(x => x.CdPadeiro == id || x.CdCliente == id);
        }

        public async Task<Pedido> GetPedidoById(Guid id)
        {
            return await _pedidoRepository.Get(x => x.CdPedido == id);
        }

        public async Task CriaPedido(Pedido pedido)
        {
            await _pedidoRepository.Insert(pedido);
            await _unitOfWork.Save();
        }
    }
}
