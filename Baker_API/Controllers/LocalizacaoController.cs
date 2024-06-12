using Baker_API.Services;
using Baker_API.Views;
using Microsoft.AspNetCore.Mvc;

namespace Baker_API.Controllers
{
    public class LocalizacaoController : Controller
    {

        [HttpGet("ListarPadeirosProximos")]
        public IActionResult ListarPadeirosProximos(string CEP_CLIENTE, int QT_LINHAS, string? LS_ALIMENTOS_RESTRITOS_PADEIRO = null)
        {
            GoogleMaps maps = new GoogleMaps();
            RetornoView retorno = new RetornoView();

            try
            {
                if (LS_ALIMENTOS_RESTRITOS_PADEIRO == "null")
                {
                    LS_ALIMENTOS_RESTRITOS_PADEIRO = null;
                }

                // Pesquisa Latitude/Longitudo do Cliente pelo CEP
                LocalizacaoView localizacao = GoogleMaps.ZipCodeSearch(CEP_CLIENTE);

                if (!string.IsNullOrEmpty(localizacao.DS_MENSAGEM))
                {
                    retorno.Mensagem = localizacao.DS_MENSAGEM;
                    return BadRequest(retorno);
                }

                // Lista de padeiros por Cidade
                Padeiros padeiro = new Padeiros();
                List<PadeiroView> lstPadeiros = padeiro.ListarPadeiros(localizacao.NM_CIDADE, LS_ALIMENTOS_RESTRITOS_PADEIRO);


                if (lstPadeiros == null || lstPadeiros.Count == 0)
                {
                    retorno.Mensagem = "Nenhum padeiro foi encontrado nas proximides!";
                    return BadRequest(retorno);
                }

                // Retornar a localização de cada padeiro próximo do cliente
                padeiro.ListarLocalizacaoPadeiro(localizacao, lstPadeiros);


                // Retornar os 3 padeiros mais próximos da localização do cliente
                retorno.Data = padeiro.ListarMelhorLocalizacao(localizacao, lstPadeiros, QT_LINHAS);

                if (retorno.Data == null)
                {
                    retorno.Mensagem = "Nenhum registro encontrado!";
                }

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Erro de Sistema";
                retorno.StackTrace = ex.Message + "/n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }
    }
}
