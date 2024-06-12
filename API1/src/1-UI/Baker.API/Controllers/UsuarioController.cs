using Baker.Application.Dtos.Usuario;
using Baker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Baker.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        /// <summary>
        /// Consulta Usuário
        /// </summary>
        /// <response code="200">Retorna Usuário</response>
        /// <response code="404">Nenhum usuário localizado</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUsuarioDto>> GetUsuario(Guid id, IUsuarioAppService appService)
        {
            try
            {
                GetUsuarioDto response = await appService.GetUsuario(id);

                if (response != null) return Ok(response);
                else return NotFound();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(404, new { message = $"Não foi encontrado nenhum usuário com o código: {id}" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no GetDados: {e.Message}" });
            }
        }

        /// <summary>
        /// Logar
        /// </summary>
        /// <response code="200">Login realizado</response>
        /// <response code="400">Erro no request</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpPost("Logar")]
        public async Task<ActionResult<Guid>> Logar([FromBody] LogarDto request, IUsuarioAppService appService)
        {
            try
            {
                Guid id = await appService.Logar(request);
                return Ok(id);
            }
            catch (ArgumentNullException)
            {
                return StatusCode(400, new { message = $"Email e/ou senha incorreto(s)" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no Logar: {e.Message}" });
            }
        }

        /// <summary>
        /// Cadastra Usuário
        /// </summary>
        /// <response code="201">Usuário cadastrado</response>
        /// <response code="400">Erro no request</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarDto request, IUsuarioAppService appService)
        {
            try
            {
                Guid id = await appService.CadastraUsuario(request);
                return CreatedAtAction(nameof(GetUsuario), new { id }, id);
            }
            catch (InvalidDataException)
            {
                return StatusCode(400, new { message = $"CPF/CNPJ inválido: {request.CpfCnpj}" });
            }
            catch (ArgumentException)
            {
                return StatusCode(400, new { message = $"Email ({request.Email}) e/ou CPF/CNPJ ({request.CpfCnpj}) já existente(s) na base" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no Cadastrar: {e.Message}" });
            }
        }

        /// <summary>
        /// Atualiza Usuário
        /// </summary>
        /// <response code="204">Usuário atualizado</response>
        /// <response code="400">Erro no request</response>
        /// <response code="500">Erro no servidor</response>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> AlterarDados([FromBody] AlterarDadosDto request, IUsuarioAppService appService)
        {
            try
            {
                await appService.UpdateUsuario(request);
                return NoContent();
            }
            catch (InvalidDataException)
            {
                return StatusCode(400, new { message = $"Senha antiga inválida" });
            }
            catch (ArgumentException)
            {
                return StatusCode(400, new { message = $"Email ({request.Email}) já existente na base" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro no AlterarDados: {e.Message}" });
            }
        }
    }
}
