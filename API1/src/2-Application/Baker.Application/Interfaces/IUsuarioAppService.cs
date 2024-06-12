using Baker.Application.Dtos.Usuario;

namespace Baker.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        Task<GetUsuarioDto> GetUsuario(Guid id);

        Task<Guid> CadastraUsuario(CadastrarDto usuario);

        Task UpdateUsuario(AlterarDadosDto usuario);

        Task<Guid> Logar(LogarDto request);
    }
}