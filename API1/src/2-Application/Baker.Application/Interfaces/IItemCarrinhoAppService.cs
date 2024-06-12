using Baker.Application.Dtos.Carrinho;

namespace Baker.Application.Interfaces
{
    public interface IItemCarrinhoAppService
    {
        Task DeletaItem(int id);

        Task InsereItem(InserirItemDto item);

        Task AlteraQuantidadeItem(AlterarQuantidadeItemDto item);

        Task<IEnumerable<GetItemCarrinhoDto>> GetItensCarrinho(Guid usuarioId);
    }
}