using Baker_API.Domains;
using Baker_API.Models;
using Baker_API.Views;

namespace Baker_API.Repository
{
    public class CarrinhoRepository
    {
        const string dbName = "DB_BAKER";

        public void Save(CarrinhoView carrinho)
        {
            Helper helper = new Helper();
            helper.ExecuteScalar(dbName, "dbo.spINSCarrinho", new
            {
                carrinho.CD_USUARIO,
                carrinho.CD_PRODUTO,
                carrinho.QT_PRODUTO,
                carrinho.VL_PRECO 
            }) ;
        }

        public void Delete(int CD_ITENS_DO_CARRINHO)
        {
            Helper helper = new Helper();
            helper.ExecuteScalar(dbName, "dbo.spDELCarrinho", new
            {
                CD_ITENS_DO_CARRINHO
            });
        }

        public List<CarrinhoModel> List(Guid CD_USUARIO)
        {
            Helper helper = new Helper();
            return helper.ExecuteList<CarrinhoModel>(dbName, "dbo.spLSTCarrinho", new
            {
                CD_USUARIO
            });

        }
    }
}
