using Baker_API.Services;
using Baker_API.Views;
using Microsoft.AspNetCore.Mvc;

namespace Baker_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarrinhoController : Controller
    {

        [HttpPost("Save")]
        public IActionResult Save([FromBody] CarrinhoView carrinhoIten)
        {
            Carrinho carr = new Carrinho();
            RetornoView retorno = new RetornoView();

            try
            {
                if (carrinhoIten == null)
                {
                    retorno.Mensagem = "Nenhum item foi incluído no Carrinho!";
                    return BadRequest(retorno);
                }

                carr.Save(carrinhoIten);

                retorno.Mensagem = "Itens do Carrinho salvo com sucesso!";

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpPut("Delete")]
        public IActionResult Delete(int CD_ITENS_DO_CARRINHO)
        {
            Carrinho carr = new Carrinho();
            RetornoView retorno = new RetornoView();

            try
            {

                carr.Delete(CD_ITENS_DO_CARRINHO);

                retorno.Mensagem = "Iten do Carrinho removido com sucesso!";

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
            Carrinho carr = new Carrinho();
            RetornoView retorno = new RetornoView();

            try
            {
                retorno.Data = carr.List(CD_USUARIO);

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
