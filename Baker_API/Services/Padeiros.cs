using Baker_API.Interfaces;
using Baker_API.Models;
using Baker_API.Views;

namespace Baker_API.Services
{
    public class Padeiros : IPadeiros
    {
        public List<PadeiroView> ListarPadeiros(string NM_CIDADE, string? LS_ALIMENTOS_RESTRITOS = null)
        {
            try
            {
                Repository.PadeiroRepository rep = new Repository.PadeiroRepository();

                List<PadeiroView> ListaPadeiros = new List<PadeiroView>();
                List<PadeiroModel> lista = rep.List(NM_CIDADE, LS_ALIMENTOS_RESTRITOS);

                foreach (PadeiroModel Padeiro in lista)
                {
                    PadeiroView model = new PadeiroView();
                    model = Converter(Padeiro);

                    ListaPadeiros.Add(model);
                }

                if (ListaPadeiros.Count() == 0)
                {
                    return null;
                }

                return ListaPadeiros;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<RelatorioView> RelatorioVendas(Guid CD_USUARIO)
        {
            try
            {
                Repository.PadeiroRepository rep = new Repository.PadeiroRepository();

                List<RelatorioView> ListaRelatorio = new List<RelatorioView>();
                List<RelatorioModel> lista = rep.Report(CD_USUARIO);

                foreach (RelatorioModel Relatorio in lista)
                {
                    RelatorioView model = new RelatorioView();
                    model = Converter(Relatorio);

                    ListaRelatorio.Add(model);
                }

                if (ListaRelatorio.Count() == 0)
                {
                    return null;
                }

                return ListaRelatorio;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListarLocalizacaoPadeiro(LocalizacaoView localizacao, List<PadeiroView> lstPadeiros)
        {
            foreach (PadeiroView item in lstPadeiros)
            {
                PadeiroModel padeiro = Converter(item);

                // Pesquisa a Latitude e Longitude de cada Padeiro.
                GoogleMaps.AddressSearch(padeiro, localizacao.NM_CIDADE);

                item.CD_LATITUDE = padeiro.CD_LATITUDE;
                item.CD_LONGITUDE = padeiro.CD_LONGITUDE;
            }

        }

        public List<PadeiroView> ListarMelhorLocalizacao(LocalizacaoView localizacao, List<PadeiroView> lstPadeiros, int QT_LINHAS)
        {
            List<PadeiroView> lstPadeiroMaisProximos = new List<PadeiroView>();

            // Ordenar os endereços pela distância até a latitude e longitude alvo
            var enderecosOrdenados = lstPadeiros.OrderBy(e => DistanciaEntrePontos(e.CD_LATITUDE, e.CD_LONGITUDE, localizacao.CD_LATITUDE, localizacao.CD_LONGITUDE));

            // Selecionar os 3 endereços mais próximos
            var enderecosMaisProximos = enderecosOrdenados.Take(QT_LINHAS);

            foreach(var endereco in enderecosMaisProximos)
            {
                lstPadeiroMaisProximos.Add(endereco);
            }


            if (lstPadeiroMaisProximos.Count() == 0)
            {
                return null;
            }

            return lstPadeiroMaisProximos;
        }

        // Função para calcular a distância entre dois pontos usando a fórmula de Haversine
        static double DistanciaEntrePontos(double lat1, double lon1, double lat2, double lon2)
        {
            const double raioTerraKm = 6371; // Raio da Terra em quilômetros
                                                       
            double dLat = GrausParaRadianos(lat2 - lat1);
            double dLon = GrausParaRadianos(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(GrausParaRadianos(lat1)) * Math.Cos(GrausParaRadianos(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distancia = raioTerraKm * c;
            return distancia;
        }

        // Função auxiliar para converter graus para radianos
        static double GrausParaRadianos(double graus)
        {
            return graus * Math.PI / 180;
        }

        private PadeiroView Converter(PadeiroModel padeiro)
        {
            PadeiroView obj = new PadeiroView();

            obj.CD_USUARIO = padeiro.CD_USUARIO;
            obj.NM_USUARIO = padeiro.NM_USUARIO;
            obj.DS_EMAIL = padeiro.DS_EMAIL;
            obj.DS_TELEFONE = padeiro.DS_TELEFONE;
            obj.NM_ESTADO = padeiro.NM_ESTADO;
            obj.NM_CIDADE = padeiro.NM_CIDADE;
            obj.DS_ENDERECO = padeiro.DS_ENDERECO;
            obj.CD_CEP = padeiro.CD_CEP;
            obj.CD_SENHA = padeiro.CD_SENHA;
            obj.CD_CPF_CNPJ = padeiro.CD_CPF_CNPJ;
            obj.CD_LATITUDE = padeiro.CD_LATITUDE;
            obj.CD_LONGITUDE = padeiro.CD_LONGITUDE;
            obj.LS_ALIMENTOS_RESTRITOS_PADEIRO = padeiro.LS_ALIMENTOS_RESTRITOS_PADEIRO;
            obj.LS_ALIMENTOS_RESTRITOS_PRODUTO = padeiro.LS_ALIMENTOS_RESTRITOS_PRODUTO;

            return obj;
        }

        private PadeiroModel Converter(PadeiroView padeiro)
        {
            PadeiroModel obj = new PadeiroModel();

            obj.CD_USUARIO = padeiro.CD_USUARIO;
            obj.NM_USUARIO = padeiro.NM_USUARIO;
            obj.DS_EMAIL = padeiro.DS_EMAIL;
            obj.DS_TELEFONE = padeiro.DS_TELEFONE;
            obj.NM_ESTADO = padeiro.NM_ESTADO;
            obj.NM_CIDADE = padeiro.NM_CIDADE;
            obj.DS_ENDERECO = padeiro.DS_ENDERECO;
            obj.CD_CEP = padeiro.CD_CEP;
            obj.CD_SENHA = padeiro.CD_SENHA;
            obj.CD_CPF_CNPJ = padeiro.CD_CPF_CNPJ;
            obj.CD_LATITUDE = padeiro.CD_LATITUDE;
            obj.CD_LONGITUDE = padeiro.CD_LONGITUDE;
            obj.LS_ALIMENTOS_RESTRITOS_PADEIRO = padeiro.LS_ALIMENTOS_RESTRITOS_PADEIRO;
            obj.LS_ALIMENTOS_RESTRITOS_PRODUTO = padeiro.LS_ALIMENTOS_RESTRITOS_PRODUTO;

            return obj;
        }

        private RelatorioModel Converter(RelatorioView relatorio)
        {
            RelatorioModel obj = new RelatorioModel();

            obj.CD_PEDIDO = relatorio.CD_PEDIDO;
            obj.NM_PADEIRO = relatorio.NM_PADEIRO;
            obj.NM_CLIENTE = relatorio.NM_CLIENTE;
            obj.DT_PEDIDO = relatorio.DT_PEDIDO;
            obj.NM_PRODUTO = relatorio.NM_PRODUTO;
            obj.QT_PRODUTO = relatorio.QT_PRODUTO;
            obj.VL_PRECO = relatorio.VL_PRECO;
            obj.VL_SUBTOTAL = relatorio.VL_SUBTOTAL;
            obj.VL_TOTAL = relatorio.VL_TOTAL;
            obj.NM_ESTADO = relatorio.NM_ESTADO;
            obj.NM_CIDADE = relatorio.NM_CIDADE;
            obj.DS_ENDERECO = relatorio.DS_ENDERECO;

            return obj;
        }

        private RelatorioView Converter(RelatorioModel relatorio) 
        {
            RelatorioView obj = new RelatorioView();

            obj.CD_PEDIDO = relatorio.CD_PEDIDO;
            obj.NM_PADEIRO = relatorio.NM_PADEIRO;
            obj.NM_CLIENTE = relatorio.NM_CLIENTE;
            obj.DT_PEDIDO = relatorio.DT_PEDIDO;
            obj.NM_PRODUTO = relatorio.NM_PRODUTO;
            obj.QT_PRODUTO = relatorio.QT_PRODUTO;
            obj.VL_PRECO = relatorio.VL_PRECO;
            obj.VL_SUBTOTAL = relatorio.VL_SUBTOTAL;
            obj.VL_TOTAL = relatorio.VL_TOTAL;
            obj.NM_ESTADO = relatorio.NM_ESTADO;
            obj.NM_CIDADE = relatorio.NM_CIDADE;
            obj.DS_ENDERECO = relatorio.DS_ENDERECO;

            return obj;
        }
    }

}

