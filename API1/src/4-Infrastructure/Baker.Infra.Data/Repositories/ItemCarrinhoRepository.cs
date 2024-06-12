using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Baker.Infra.Data.Repositories
{
    public class ItemCarrinhoRepository : IItemCarrinhoRepository
    {
        private readonly BakerDbContext _context;

        public ItemCarrinhoRepository(BakerDbContext context)
        {
            _context = context;
        }

        public async Task<ItemCarrinho> Get(Expression<Func<ItemCarrinho, bool>> filter)
        {
            return await _context.ItensCarrinho.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<ItemCarrinho>> GetAllByCarrinhoId(Expression<Func<ItemCarrinho, bool>> filter)
        {
            return await _context.ItensCarrinho.Where(filter).ToListAsync();
        }

        public async Task Delete(ItemCarrinho item)
        {
            await Task.FromResult(_context.ItensCarrinho.Remove(item));
        }

        public async Task Delete(IEnumerable<ItemCarrinho> itens)
        {
            _context.ItensCarrinho.RemoveRange(itens);
        }

        public async Task Update(ItemCarrinho item)
        {
            await Task.FromResult(_context.ItensCarrinho.Entry(item).State = EntityState.Modified);
        }

        public async Task Insert(ItemCarrinho item)
        {
            await Task.FromResult(_context.ItensCarrinho.AddAsync(item));
        }
    }
}
