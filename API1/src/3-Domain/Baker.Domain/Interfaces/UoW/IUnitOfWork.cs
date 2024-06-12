using Baker.Domain.Interfaces.Repositories;

namespace Baker.Domain.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        IUsuarioRepository UsuarioRepository { get; }

        ICarrinhoRepository CarrinhoRepository { get; }

        IItemCarrinhoRepository ItemCarrinhoRepository { get; }

        IPedidoRepository PedidoRepository { get; }

        IItemPedidoRepository ItemPedidoRepository { get; }

        IProdutoRepository ProdutoRepository { get; }

        Task Save();
    }
}
