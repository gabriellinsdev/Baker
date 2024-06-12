using Baker_API.Views;

namespace Baker_API.Interfaces
{
    public interface IPadeiros
    {
        List<PadeiroView> ListarPadeiros(string NM_CIDADE, string LS_ALIMENTOS_RESTRITOS);
        void ListarLocalizacaoPadeiro(LocalizacaoView localizacao, List<PadeiroView> lstPadeiros);
        List<PadeiroView> ListarMelhorLocalizacao(LocalizacaoView localizacao, List<PadeiroView> lstPadeiros, int QT_LINHAS);

    }
}
