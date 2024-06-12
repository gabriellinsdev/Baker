using Baker_API.Domains;
using Baker_API.Models;
using Baker_API.Views;

namespace Baker_API.Services
{
    public class Usuario
    {
        public UsuarioDetailView GetUsuario(Guid CD_USUARIO)
        {
            try
            {
                Repository.UsuarioRepository rep = new Repository.UsuarioRepository();

                UsuarioModel Usuario = rep.GetUsuario(CD_USUARIO);
                UsuarioDetailView viewUsuario = ConverterDetail(Usuario);

                return viewUsuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private UsuarioModel Converter(UsuarioView usuario)
        {
            UsuarioModel obj = new UsuarioModel();

            obj.CD_USUARIO = usuario.CD_USUARIO;
            obj.NM_USUARIO = usuario.NM_USUARIO;
            obj.DS_EMAIL = usuario.DS_EMAIL;
            obj.DS_TELEFONE = usuario.DS_TELEFONE;
            obj.NM_ESTADO = usuario.NM_ESTADO;
            obj.NM_CIDADE = usuario.NM_CIDADE;
            obj.DS_ENDERECO = usuario.DS_ENDERECO;
            obj.CD_CEP = usuario.CD_CEP;
            obj.CD_SENHA = usuario.CD_SENHA;
            obj.CD_CPF_CNPJ = usuario.CD_CPF_CNPJ;
            obj.TP_USUARIO = usuario.TP_USUARIO;

            return obj;
        }

        private UsuarioDetailView ConverterDetail(UsuarioModel usuario)
        {
            UsuarioDetailView obj = new UsuarioDetailView();

            obj.CD_USUARIO = usuario.CD_USUARIO;
            obj.NM_USUARIO = usuario.NM_USUARIO;
            obj.TP_USUARIO = usuario.TP_USUARIO;

            return obj;
        }

    }
}
