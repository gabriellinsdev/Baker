using Baker_API.Domains;
using Baker_API.Interfaces;
using Baker_API.Views;
using static System.Net.Mime.MediaTypeNames;

namespace Baker_API.Services
{
    public class Produtos : IProdutos
    {

        public void Insert(ProdutoView produto)
        {
            try
            {
                Repository.ProdutoRepository rep = new Repository.ProdutoRepository();
                rep.Insert(Converter(produto));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ProdutoView produto)
        {
            try
            {
                Repository.ProdutoRepository rep = new Repository.ProdutoRepository();
                rep.Update(Converter(produto));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int CD_PRODUTO)
        {
            try
            {
                Repository.ProdutoRepository rep = new Repository.ProdutoRepository();
                rep.Delete(CD_PRODUTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProdutoDetailView> List(Guid CD_USUARIO)
        {
            try
            {
                Repository.ProdutoRepository rep = new Repository.ProdutoRepository();

                List<ProdutoModel> lista = rep.List(CD_USUARIO);
                List<ProdutoDetailView> ListaProdutos = new List<ProdutoDetailView>();

                foreach (ProdutoModel produtoModel in lista)
                {
                    ProdutoDetailView viewProduto = new ProdutoDetailView();
                    viewProduto = Converter(produtoModel);

                    ListaProdutos.Add(viewProduto);
                }

                if (ListaProdutos.Count() == 0)
                {
                    return null;
                }

                return ListaProdutos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProdutoDetailView GetProduct(int CD_PRODUTO)
        {
            try
            {
                Repository.ProdutoRepository rep = new Repository.ProdutoRepository();

                ProdutoModel produto = rep.GetProduct(CD_PRODUTO);
                ProdutoDetailView viewProduto = Converter(produto);

                return viewProduto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ProdutoModel Converter(ProdutoView produto)
        {
            ProdutoModel obj = new ProdutoModel();
            

            if (produto.FF_IMAGEM != null) {
                byte[]? VB_IMAGEM = ConvertToByte(produto.FF_IMAGEM);
                obj.VB_IMAGEM = VB_IMAGEM;
            }

            obj.CD_PRODUTO = produto.CD_PRODUTO;
            obj.DS_PRODUTO = produto.DS_PRODUTO;
            obj.NM_PRODUTO = produto.NM_PRODUTO;
            obj.VL_PRECO = produto.VL_PRECO;
            obj.CD_USUARIO = produto.CD_USUARIO;
            obj.LS_ALIMENTOS_RESTRITOS_PADEIRO = produto.LS_ALIMENTOS_RESTRITOS_PADEIRO;
            obj.LS_ALIMENTOS_RESTRITOS_PRODUTO = produto.LS_ALIMENTOS_RESTRITOS_PRODUTO;

            return obj;
        }

        private ProdutoDetailView Converter(ProdutoModel produto)
        {
            ProdutoDetailView obj = new ProdutoDetailView();

            obj.CD_PRODUTO = produto.CD_PRODUTO;
            obj.DS_PRODUTO = produto.DS_PRODUTO;
            obj.NM_PRODUTO = produto.NM_PRODUTO;
            obj.VL_PRECO = produto.VL_PRECO;
            obj.CD_USUARIO = produto.CD_USUARIO;
            obj.VB_IMAGEM = produto.VB_IMAGEM;
            obj.LS_ALIMENTOS_RESTRITOS_PADEIRO = produto.LS_ALIMENTOS_RESTRITOS_PADEIRO;
            obj.LS_ALIMENTOS_RESTRITOS_PRODUTO = produto.LS_ALIMENTOS_RESTRITOS_PRODUTO;

            return obj;
        }

        private byte[] ConvertToByte(IFormFile imagem)
        {
            byte[] imagemBytes;
            using (var stream = imagem.OpenReadStream())
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    imagemBytes = memoryStream.ToArray();
                }
            }

            return imagemBytes;
        }
    }
}
