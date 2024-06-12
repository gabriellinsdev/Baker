using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;

namespace Baker.Domain.Services
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICarrinhoRepository _carrinhoRepository;

        public CarrinhoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _carrinhoRepository = _unitOfWork.CarrinhoRepository;
        }

        public async Task<Carrinho> GetCarrinhoByUsuarioId(Guid id)
        {
            return await _carrinhoRepository.Get(x => x.CdCliente == id);
        }

        public async Task<int> CriaCarrinho(Carrinho carrinho)
        {
            await _carrinhoRepository.Insert(carrinho);
            await _unitOfWork.Save();
            return carrinho.CdCarrinho;
        }
    }
}
