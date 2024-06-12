using Baker.Domain.Entities;
using System.Linq.Expressions;

namespace Baker.Domain.Interfaces.Repositories
{
    public interface ICarrinhoRepository
    {
        Task<Carrinho> Get(Expression<Func<Carrinho, bool>> filter);

        Task Insert(Carrinho carrinho);
    }
}