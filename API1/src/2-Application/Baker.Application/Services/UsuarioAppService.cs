using Baker.Application.Dtos.Usuario;
using Baker.Application.Interfaces;
using Baker.Application.Parsers.Usuario;
using Baker.Application.Validators;
using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Services;

namespace Baker.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioAppService(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<GetUsuarioDto> GetUsuario(Guid id)
        {
            Usuario usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario is null) throw new ArgumentNullException();
            return await ParserGetDadosDto.Parse(usuario);
        }

        public async Task<Guid> Logar(LogarDto request)
        {
            Usuario usuario = await _usuarioService.GetUsuarioByEmailAndSenha(request.Email, request.Senha);
            if (usuario is null) throw new ArgumentNullException();
            return usuario.CdUsuario;
        }

        public async Task<Guid> CadastraUsuario(CadastrarDto usuario)
        {
            try
            {
                if (await DataValidator.CpfCnpjValidator(usuario.CpfCnpj))
                {
                    Guid id = await _usuarioService.CadastraUsuario(await ParserCadastrarDto.Parse(usuario));
                    return id;
                }
                else throw new InvalidDataException();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException();
            }
        }

        public async Task UpdateUsuario(AlterarDadosDto usuario)
        {
            try
            {
                Usuario retorno = await _usuarioService.GetUsuarioBySenha(usuario.SenhaAntiga);

                if (retorno is not null)
                {
                    await _usuarioService.UpdateUsuario(await ParserAlterarDadosDto.Parse(usuario, retorno));
                }
                else throw new InvalidDataException();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException();
            }
        }
    }
}
