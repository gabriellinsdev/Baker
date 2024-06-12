using Baker.Domain.Interfaces.Repositories;
using Baker.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Baker.Infra.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly BakerDbContext _context;

        public DashboardRepository(BakerDbContext context)
        {
            _context = context;
        }

        public IEnumerable<object> GetTopProdutos(Guid id)
        {
            var query = from itemPedido in _context.ItensPedido
                        join pedido in _context.Pedidos on itemPedido.CdPedido equals pedido.CdPedido
                        join usuario in _context.Usuarios on pedido.CdPadeiro equals usuario.CdUsuario
                        join produto in _context.Produtos on itemPedido.CdProduto equals produto.CdProduto
                        group new { itemPedido, produto, usuario } by new { produto.CdProduto, produto.NmProduto, usuario.NmUsuario } into grp
                        select new
                        {
                            NomePadeiro = grp.Key.NmUsuario,
                            NomeProduto = grp.Key.NmProduto,
                            QuantidadeVendida = grp.Sum(x => x.itemPedido.QtProduto)
                        };

            return query.ToList();
        }

        public IEnumerable<object> GetQuantidadeVendas(Guid id)
        {
            return _context.Pedidos.Include(p => p.Padeiro)
                .Where(x => x.CdPadeiro == id && x.DtPedido >= DateTime.Now.AddDays(-30))
                .Join(
                    _context.Usuarios,
                    pedido => pedido.CdPadeiro,
                    usuario => usuario.CdUsuario,
                    (pedido, usuario) => new { Pedido = pedido, Usuario = usuario }
                    )
                .GroupBy(x => new { x.Pedido.CdPadeiro, x.Usuario.NmUsuario })
                .Select(x => new { NomePadeiro = x.Key.NmUsuario, QuantidadeVendas = x.Count() });
        }
    }
}
