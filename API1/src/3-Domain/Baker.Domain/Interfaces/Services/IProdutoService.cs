using Baker.Domain.Entities;

namespace Baker.Domain.Interfaces.Services
{
    public interface IProdutoService
    {
        Task<Produto> GetProdutoById(int id);
        Task<IEnumerable<Produto>> GetAll();

    }
}