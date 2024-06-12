using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Baker.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BakerDbContext _context;

        public UsuarioRepository(BakerDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Get(Expression<Func<Usuario, bool>> filter)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(filter);
        }

        public async Task Update(Usuario usuario)
        {
            await Task.FromResult(_context.Usuarios.Entry(usuario).State = EntityState.Modified);
        }

        public async Task Insert(Usuario usuario)
        {
            await Task.FromResult(_context.Usuarios.AddAsync(usuario));
        }
    }
}
