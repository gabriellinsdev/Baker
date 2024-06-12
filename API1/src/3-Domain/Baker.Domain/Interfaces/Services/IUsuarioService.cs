using Baker.Domain.Entities;

namespace Baker.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarioById(Guid id);

        Task<Usuario> GetUsuarioByEmail(string email);

        Task<Guid> CadastraUsuario(Usuario usuario);

        Task UpdateUsuario(Usuario usuario);

        Task<Usuario> GetUsuarioBySenha(string senha);

        Task<Usuario> GetUsuarioByEmailAndSenha(string email, string senha);
    }
}