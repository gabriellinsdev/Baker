using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;

namespace Baker.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _usuarioRepository = _unitOfWork.UsuarioRepository;
        }

        public async Task<Usuario> GetUsuarioById(Guid id)
        {
            return await _usuarioRepository.Get(x => x.CdUsuario == id);
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            return await _usuarioRepository.Get(x => x.DsEmail == email);
        }

        public async Task<Usuario> GetUsuarioByEmailAndSenha(string email, string senha)
        {
            return await _usuarioRepository.Get(x => x.DsEmail == email && x.CdSenha == senha);
        }

        public async Task<Usuario> GetUsuarioBySenha(string senha)
        {
            Usuario usuario = await _usuarioRepository.Get(x => x.CdSenha == senha);
            return usuario;
        }

        public async Task<Guid> CadastraUsuario(Usuario usuario)
        {
            Usuario retorno = await _usuarioRepository.Get(x => x.DsEmail == usuario.DsEmail || x.CdCpfCnpj == usuario.CdCpfCnpj);

            if (retorno is null)
            {
                await _usuarioRepository.Insert(usuario);
                await _unitOfWork.Save();
                return usuario.CdUsuario;
            }
            else throw new ArgumentException();
        }

        public async Task UpdateUsuario(Usuario usuario)
        {
            Usuario retorno = await _usuarioRepository.Get(x => x.DsEmail == usuario.DsEmail);

            if (retorno is not null)
            {
                if (usuario.CdUsuario == retorno.CdUsuario)
                {
                    await _usuarioRepository.Update(usuario);
                    await _unitOfWork.Save();
                }
                else throw new ArgumentException();
            }
            else
            {
                await _usuarioRepository.Update(usuario);
                await _unitOfWork.Save();
            }
        }
    }
}
