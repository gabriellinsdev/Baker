using Baker.Domain.Entities;
using System.Linq.Expressions;

namespace Baker.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> Get(Expression<Func<Usuario, bool>> filter);

        Task Update(Usuario usuario);

        Task Insert(Usuario usuario);
    }
}
