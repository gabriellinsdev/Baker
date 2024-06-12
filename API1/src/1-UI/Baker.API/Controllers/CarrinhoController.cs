using Baker.Application.Dtos.Carrinho;
using Baker.Application.Dtos.Pedido;
using Baker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Baker.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        /// <summary>
        /// Consulta Itens do carrinho Por id do Usuario
        /// </summary>
        /// <response code="200">Retorna itens</response>
        /// <response code="404">Nenhum item localizado</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpGet("Itens/{usuarioId}")]
        public async Task<ActionResult<GetItemCarrinhoDto>> GetItensByUsuarioId(Guid usuarioId, IItemCarrinhoAppService appService)
        {
            try
            {
                IEnumerable<GetItemCarrinhoDto> response = await appService.GetItensCarrinho(usuarioId);

                if (response != null && response.Any()) return Ok(response);
                else return NotFound();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(404, new { message = $"Não foi encontrado nenhum item para o usuário: {usuarioId}" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no GetItensByUsuarioId: {e.Message}" });
            }
        }

        /// <summary>
        /// Insere item no carrinho
        /// </summary>
        /// <response code="201">Item inserido no carrinho</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InserirItem([FromBody] InserirItemDto request, IItemCarrinhoAppService appService)
        {
            try
            {
                await appService.InsereItem(request);
                return StatusCode(201, new { message = $"Item inserido no carrinho" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no InserirItem: {e.Message}" });
            }
        }

        /// <summary>
        /// Altera quantidade do item no carrinho
        /// </summary>
        /// <response code="204">Quantidade do item alterada</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> AlterarQuantidadeItem([FromBody] AlterarQuantidadeItemDto request, IItemCarrinhoAppService appService)
        {
            try
            {
                await appService.AlteraQuantidadeItem(request);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no AlterarQuantidadeItem: {e.Message}" });
            }
        }

        /// <summary>
        /// Exclui item do carrinho
        /// </summary>
        /// <response code="204">Item excluído</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirItem(int id, IItemCarrinhoAppService appService)
        {
            try
            {
                await appService.DeletaItem(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no ExcluirItem: {e.Message}" });
            }
        }
    }
}
