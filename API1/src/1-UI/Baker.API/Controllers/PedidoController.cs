using Baker.Application.Dtos.Pedido;
using Baker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Baker.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        /// <summary>
        /// Consulta Pedidos Por Id do Usuario
        /// </summary>
        /// <response code="200">Retorna Pedidos</response>
        /// <response code="404">Nenhum usuário localizado</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpGet("Usuario/{id}")]
        public async Task<ActionResult<GetPedidosDto>> GetPedidosByUsuarioId(Guid id, IPedidoAppService appService)
        {
            try
            {
                IEnumerable<GetPedidosDto> response = await appService.GetPedidosByUsuarioId(id);

                if (response != null && response.Any()) return Ok(response);
                else return NotFound();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(404, new { message = $"Não foi encontrado nenhum pedido para o usuário: {id}" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no GetPedidos: {e.Message}" });
            }
        }

        /// <summary>
        /// Consulta Pedido
        /// </summary>
        /// <response code="200">Retorna Pedido</response>
        /// <response code="404">Nenhum usuário localizado</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPedidosDto>> GetPedido(Guid id, IPedidoAppService appService)
        {
            try
            {
                GetPedidosDto response = await appService.GetPedido(id);

                if (response != null) return Ok(response);
                else return NotFound();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(404, new { message = $"Não foi encontrado nenhum pedido com o código: {id}" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no GetPedido: {e.Message}" });
            }
        }

        /// <summary>
        /// Cria Pedido
        /// </summary>
        /// <response code="201">Pedido criado</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CriarPedidoDtoResponse>> CriarPedido([FromBody] CriarPedidoDtoRequest request, IPedidoAppService appService)
        {
            try
            {
                CriarPedidoDtoResponse response = await appService.CriaPedido(request);
                return CreatedAtAction(nameof(GetPedido), new { id = response.CodigoPedido }, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no CriarPedido: {e.Message}" });
            }
        }
    }
}
