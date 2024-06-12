using Baker_API.Views;

namespace Baker_API.Interfaces
{
    public interface ICarrinho
    {
        void Save(CarrinhoView carrinho);
        void Delete(int CD_ITENS_DO_CARRINHO);
        List<CarrinhoView> List(Guid CD_USUARIO);
    }
}

