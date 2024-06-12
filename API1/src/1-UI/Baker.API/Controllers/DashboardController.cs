using Baker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Baker.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        /// <summary>
        /// Consulta Top Produtos Vendidos
        /// </summary>
        /// <response code="200">Retorna top produtos vendidos</response>
        /// <response code="404">Nenhum produto vendido localizado</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpGet("TopProdutos/{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopProdutos(Guid id, IDashboardAppService appService)
        {
            try
            {
                var response = appService.GetTopProdutos(id);

                if (response != null) return Ok(response);
                else return NotFound();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(404, new { message = $"Não foi encontrado nenhum produto vendido pelo usuário com o código: {id}" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no GetTopProdutos: {e.Message}" });
            }
        }

        /// <summary>
        /// Consulta Quantidade de Vendas
        /// </summary>
        /// <response code="200">Retorna quantidade de vendas</response>
        /// <response code="404">Nenhum venda localizada</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpGet("QuantidadeVendas/{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuantidadeVendas(Guid id, IDashboardAppService appService)
        {
            try
            {
                var response = appService.GetQuantidadeVendas(id);

                if (response != null) return Ok(response);
                else return NotFound();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(404, new { message = $"Não foi encontrado nenhuma venda pelo usuário com o código: {id}" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no GetQuantidadeVendas: {e.Message}" });
            }

        }
    }
}
