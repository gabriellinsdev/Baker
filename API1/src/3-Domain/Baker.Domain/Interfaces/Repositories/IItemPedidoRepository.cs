using Baker.Domain.Entities;
using System.Linq.Expressions;

namespace Baker.Domain.Interfaces.Repositories
{
    public interface IItemPedidoRepository
    {
        Task Insert(IEnumerable<ItemPedido> item);

        Task<IEnumerable<ItemPedido>> GetAllById(Expression<Func<ItemPedido, bool>> filter);
    }
}