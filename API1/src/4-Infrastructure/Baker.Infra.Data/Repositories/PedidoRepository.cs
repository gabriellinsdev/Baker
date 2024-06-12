using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Baker.Infra.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly BakerDbContext _context;

        public PedidoRepository(BakerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetAllById(Expression<Func<Pedido, bool>> filter)
        {
            return await _context.Pedidos.Where(filter).ToListAsync();
        }

        public async Task<Pedido> Get(Expression<Func<Pedido, bool>> filter)
        {
            return await _context.Pedidos.FirstOrDefaultAsync(filter);
        }

        public async Task Insert(Pedido pedido)
        {
            await Task.FromResult(_context.Pedidos.AddAsync(pedido));
        }
    }
}
