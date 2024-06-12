using Baker.Domain.Entities;

namespace Baker.Domain.Interfaces.Services
{
    public interface ICarrinhoService
    {
        Task<Carrinho> GetCarrinhoByUsuarioId(Guid id);

        Task<int> CriaCarrinho(Carrinho carrinho);
    }
}