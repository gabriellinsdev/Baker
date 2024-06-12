using Baker.Domain.Entities;
using System.Linq.Expressions;

namespace Baker.Domain.Interfaces.Repositories
{
    public interface IProdutoRepository
    {
        Task<Produto> Get(Expression<Func<Produto, bool>> filter);
        Task<IEnumerable<Produto>> GetAll();

    }
}
