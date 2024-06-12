using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.UoW;
using Baker.Infra.Data.Context;
using Baker.Infra.Data.Repositories;

namespace Baker.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BakerDbContext _context;
        private IUsuarioRepository _usuarioRepository = null;
        private ICarrinhoRepository _carrinhoRepository = null;
        private IItemCarrinhoRepository _itemCarrinhoRepository = null;
        private IPedidoRepository _pedidoRepository = null;
        private IItemPedidoRepository _itemPedidoRepository = null;
        private IProdutoRepository _produtoRepository = null;
        public UnitOfWork(BakerDbContext context)
        {
            _context = context;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public IUsuarioRepository UsuarioRepository
        {
            get
            {
                if (_usuarioRepository is null) _usuarioRepository = new UsuarioRepository(_context);
                return _usuarioRepository;
            }
        }

        public ICarrinhoRepository CarrinhoRepository
        {
            get
            {
                if (_carrinhoRepository is null) _carrinhoRepository = new CarrinhoRepository(_context);
                return _carrinhoRepository;
            }
        }

        public IItemCarrinhoRepository ItemCarrinhoRepository
        {
            get
            {
                if (_itemCarrinhoRepository is null) _itemCarrinhoRepository = new ItemCarrinhoRepository(_context);
                return _itemCarrinhoRepository;
            }
        }

        public IPedidoRepository PedidoRepository
        {
            get
            {
                if (_pedidoRepository is null) _pedidoRepository = new PedidoRepository(_context);
                return _pedidoRepository;
            }
        }

        public IItemPedidoRepository ItemPedidoRepository
        {
            get
            {
                if (_itemPedidoRepository is null) _itemPedidoRepository = new ItemPedidoRepository(_context);
                return _itemPedidoRepository;
            }
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                if (_produtoRepository is null) _produtoRepository = new ProdutoRepository(_context);
                return _produtoRepository;
            }
        }
    }
}
