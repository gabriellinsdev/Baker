using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;

namespace Baker.Domain.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _produtoRepository = _unitOfWork.ProdutoRepository;
        }

        public async Task<Produto> GetProdutoById(int id)
        {
            return await _produtoRepository.Get(x => x.CdProduto == id);
        }

        public async Task<IEnumerable<Produto>> GetAll()
        {
            return await _produtoRepository.GetAll();
        }
    }
}
