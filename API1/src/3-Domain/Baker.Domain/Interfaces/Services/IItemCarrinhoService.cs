using Baker.Domain.Entities;

namespace Baker.Domain.Interfaces.Services
{
    public interface IItemCarrinhoService
    {
        Task InsereItem(ItemCarrinho item);

        Task AtualizaQuantidadeItem(ItemCarrinho item);

        Task DeletaItem(ItemCarrinho item);

        Task<ItemCarrinho> GetItemCarrinhoById(int id);

        Task<IEnumerable<ItemCarrinho>> GetItemCarrinhoByCarrinhoId(int id);

        Task DeletaItens(IEnumerable<ItemCarrinho> itens);
    }
}