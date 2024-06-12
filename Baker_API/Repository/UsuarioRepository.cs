using Baker_API.Models;

namespace Baker_API.Repository
{
    public class UsuarioRepository
    {

        const string dbName = "DB_BAKER";

        public UsuarioModel GetUsuario(Guid CD_USUARIO)
        {
            Helper helper = new Helper();
            return helper.ConvertDataTable<UsuarioModel>(helper.ExecuteTable(dbName, "dbo.spSELUsuario", new
            {
                CD_USUARIO
            }));

        }

    }
}
