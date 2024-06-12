using Baker.Domain.Entities;
using System.Linq.Expressions;

namespace Baker.Domain.Interfaces.Repositories
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> GetAllById(Expression<Func<Pedido, bool>> filter);

        Task<Pedido> Get(Expression<Func<Pedido, bool>> filter);

        Task Insert(Pedido pedido);
    }
}
