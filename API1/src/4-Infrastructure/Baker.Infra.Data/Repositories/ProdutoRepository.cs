using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Baker.Infra.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly BakerDbContext _context;

        public ProdutoRepository(BakerDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> Get(Expression<Func<Produto, bool>> filter)
        {
            return await _context.Produtos.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<Produto>> GetAll()
        {
            return _context.Produtos;
        }
    }
}
