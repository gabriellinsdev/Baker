using Baker.Domain.Entities;
using System.Linq.Expressions;

namespace Baker.Domain.Interfaces.Repositories
{
    public interface IItemCarrinhoRepository
    {
        Task Update(ItemCarrinho item);

        Task Insert(ItemCarrinho item);

        Task<ItemCarrinho> Get(Expression<Func<ItemCarrinho, bool>> filter);

        Task<IEnumerable<ItemCarrinho>> GetAllByCarrinhoId(Expression<Func<ItemCarrinho, bool>> filter);

        Task Delete(ItemCarrinho item);

        Task Delete(IEnumerable<ItemCarrinho> itens);
    }
}