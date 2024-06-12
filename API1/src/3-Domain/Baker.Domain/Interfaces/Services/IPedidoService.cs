using Baker.Domain.Entities;

namespace Baker.Domain.Interfaces.Services
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetPedidosByUsuarioId(Guid id);

        Task<Pedido> GetPedidoById(Guid id);

        Task CriaPedido(Pedido pedido);
    }
}