using Baker_API.Models;

namespace Baker_API.Repository
{
    public class PadeiroRepository
    {

        const string dbName = "DB_BAKER";

        public List<PadeiroModel> List(string NM_CIDADE, string? LS_ALIMENTOS_RESTRITOS = null)
        {
            Helper helper = new Helper();
            return helper.ExecuteList<PadeiroModel>(dbName, "dbo.spLSTLocalizacaoPadeiros", new
            {
                NM_CIDADE,
                LS_ALIMENTOS_RESTRITOS
            });

        }

        public List<RelatorioModel> Report(Guid CD_USUARIO)
        {
            Helper helper = new Helper();
            return helper.ExecuteList<RelatorioModel>(dbName, "dbo.spRPTVendasPadeiros", new
            {
                CD_USUARIO
            });

        }
    }
}
