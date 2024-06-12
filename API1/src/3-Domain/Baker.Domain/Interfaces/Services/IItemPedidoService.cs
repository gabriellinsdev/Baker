using Baker.Domain.Entities;

namespace Baker.Domain.Interfaces.Services
{
    public interface IItemPedidoService
    {
        Task InsereItens(IEnumerable<ItemPedido> itens);

        Task<IEnumerable<ItemPedido>> GetItemPedidoByPedidoId(Guid id);
    }
}