using Baker_API.Views;

namespace Baker_API.Interfaces
{
    public interface IProdutos
    {
        public void Insert(ProdutoView produtos);

        public void Update(ProdutoView produtos);

        public void Delete(int idProduto);

        public List<ProdutoDetailView> List(Guid usuario);
    }
}
