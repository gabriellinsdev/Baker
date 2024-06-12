using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Baker.Infra.Data.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly BakerDbContext _context;

        public CarrinhoRepository(BakerDbContext context)
        {
            _context = context;
        }

        public async Task<Carrinho> Get(Expression<Func<Carrinho, bool>> filter)
        {
            return await _context.Carrinhos.FirstOrDefaultAsync(filter);
        }

        public async Task Insert(Carrinho carrinho)
        {
            await Task.FromResult(_context.Carrinhos.AddAsync(carrinho));
        }
    }
}
