using Baker_API.Services;
using Baker_API.Views;
using Microsoft.AspNetCore.Mvc;

namespace Baker_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {

        [HttpPost("Insert")]
        public IActionResult Insert([FromForm] ProdutoView produto)
        {
            Produtos prod = new Produtos();
            RetornoView retorno = new RetornoView();

            try
            {
                prod.Insert(produto);

                retorno.Mensagem = "Salvo com sucesso!";
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] ProdutoView produto)
        {
            Produtos prod = new Produtos();
            RetornoView retorno = new RetornoView();

            try
            {
                prod.Update(produto);

                retorno.Mensagem = "Salvo com sucesso!";
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int CD_PRODUTO)
        {
            Produtos prod = new Produtos();
            RetornoView retorno = new RetornoView();

            try
            {
                prod.Delete(CD_PRODUTO);

                retorno.Mensagem = "Exclusão efetuada com sucesso!";
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpGet("List")]
        public IActionResult List(Guid CD_USUARIO)
        {
            Produtos prod = new Produtos();
            RetornoView retorno = new RetornoView();

            try
            {
                retorno.Data = prod.List(CD_USUARIO);

                if (retorno.Data == null)
                {
                    retorno.Mensagem = "Nenhum registro encontrado!";
                }

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpGet("Get")]
        public IActionResult Get(int CD_PRODUTO)
        {
            Produtos prod = new Produtos();
            RetornoView retorno = new RetornoView();

            try
            {
                retorno.Data = prod.GetProduct(CD_PRODUTO);

                if (retorno.Data == null)
                {
                    retorno.Mensagem = "Nenhum registro encontrado!";
                }

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }
    }
}
