using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Baker.Infra.Data.Repositories
{
    public class ItemPedidoRepository : IItemPedidoRepository
    {
        private readonly BakerDbContext _context;

        public ItemPedidoRepository(BakerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemPedido>> GetAllById(Expression<Func<ItemPedido, bool>> filter)
        {
            return await _context.ItensPedido.Where(filter).ToListAsync();
        }

        public async Task Insert(IEnumerable<ItemPedido> item)
        {
            await Task.FromResult(_context.ItensPedido.AddRangeAsync(item));
        }
    }
}
