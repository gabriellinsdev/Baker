using Baker_API.Interfaces;
using Baker_API.Models;
using Baker_API.Views;
using System.Xml;

namespace Baker_API.Services
{
    public class Carrinho : ICarrinho
    {
        public void Save(CarrinhoView carrinho)
        {
            try
            {
                Repository.CarrinhoRepository rep = new Repository.CarrinhoRepository();

                rep.Save(carrinho);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int CD_ITENS_DO_CARRINHO)
        {
            try
            {
                Repository.CarrinhoRepository rep = new Repository.CarrinhoRepository();

                rep.Delete(CD_ITENS_DO_CARRINHO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CarrinhoView> List(Guid CD_USUARIO)
        {
            try
            {
                Repository.CarrinhoRepository rep = new Repository.CarrinhoRepository();

                List<CarrinhoModel> lista = rep.List(CD_USUARIO);
                List<CarrinhoView> Listacarrinhos = new List<CarrinhoView>();

                foreach (CarrinhoModel carrinhoModel in lista)
                {
                    CarrinhoView model = new CarrinhoView();
                    model = Converter(carrinhoModel);

                    Listacarrinhos.Add(model);
                }

                if (Listacarrinhos.Count() == 0)
                {
                    return null;
                }

                return Listacarrinhos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private CarrinhoModel Converter(CarrinhoView carrinho)
        {
            CarrinhoModel obj = new CarrinhoModel();

            obj.CD_USUARIO = carrinho.CD_USUARIO;
            obj.CD_PRODUTO = carrinho.CD_PRODUTO;
            obj.QT_PRODUTO = carrinho.QT_PRODUTO;

            return obj;
        }

        private CarrinhoView Converter(CarrinhoModel carrinho)
        {
            CarrinhoView obj = new CarrinhoView();

            obj.CD_USUARIO = carrinho.CD_USUARIO;
            obj.CD_PRODUTO = carrinho.CD_PRODUTO;
            obj.QT_PRODUTO = carrinho.QT_PRODUTO;

            return obj;
        }

        private string XmlCreator(List<CarrinhoView> carrinhoItens)
        {
            // Criando um documento XML
            XmlDocument xmlDoc = new XmlDocument();

            // Criando o nó raiz
            XmlElement root = xmlDoc.CreateElement("Carrinho");
            xmlDoc.AppendChild(root);

            // Adicionando os itens do carrinho
            foreach (var item in carrinhoItens)
            {
                // Criando o nó do carrinho
                XmlElement carrinhoElement = xmlDoc.CreateElement("Item");
                root.AppendChild(carrinhoElement);

                // Adicionando os campos ao nó do carrinho
                XmlElement cdCarrinhoElement = xmlDoc.CreateElement("CD_USUARIO");
                cdCarrinhoElement.InnerText = item.CD_USUARIO.ToString();
                carrinhoElement.AppendChild(cdCarrinhoElement);

                XmlElement cdProdutoElement = xmlDoc.CreateElement("CD_PRODUTO");
                cdProdutoElement.InnerText = item.CD_PRODUTO.ToString();
                carrinhoElement.AppendChild(cdProdutoElement);

                XmlElement qtProdutoElement = xmlDoc.CreateElement("QT_PRODUTO");
                qtProdutoElement.InnerText = item.QT_PRODUTO.ToString();
                carrinhoElement.AppendChild(qtProdutoElement);

                XmlElement vlPrecoElement = xmlDoc.CreateElement("VL_PRECO");
                vlPrecoElement.InnerText = item.VL_PRECO.ToString();
                carrinhoElement.AppendChild(vlPrecoElement);
            }

            // Salvar o XML para um arquivo ou imprimir na tela
            return xmlDoc.OuterXml;
        }

    }
}
