using Baker_API.Models;
using Baker_API.Views;
using Newtonsoft.Json;
using System.Net;

namespace Baker_API.Services
{
    public class GoogleMaps
    {

        // Insira sua chave da API do Google Maps aqui
        const string apiKey = "AIzaSyB58e1LL9TzxjAB_4kF65TWdx1bYz4kIo4";

        public static LocalizacaoView ZipCodeSearch(string CD_CEP)
        {
            LocalizacaoView loc = new LocalizacaoView();

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={CD_CEP}&key={apiKey}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                // Criar classe para representar a estrutura do JSON
                GoogleMapsResponse response = JsonConvert.DeserializeObject<GoogleMapsResponse>(json);

                if (response.status == "OK")
                {
                    Result firstResult = response.results[0];

                    // Exibir o nome da cidade
                    foreach (var component in firstResult.address_components)
                    {
                        if (Array.Exists(component.types, t => t == "administrative_area_level_2"))
                        {
                            loc.NM_CIDADE = component.long_name.ToUpper();
                            break;
                        }
                    }

                    // Exibir a latitude e a longitude

                    loc.CD_LATITUDE = firstResult.geometry.location.lat;
                    loc.CD_LONGITUDE = firstResult.geometry.location.lng;
                }
                else
                {
                    loc.DS_MENSAGEM = "Nenhum endereço encontrado ou CEP Inválido!";
                }
            }

            return loc;
        }

        public static void AddressSearch(PadeiroModel padeiro, string NM_CIDADE)
        {
            LocalizacaoView loc = new LocalizacaoView();

            string endereco = "";

            if (!string.IsNullOrEmpty(padeiro.DS_ENDERECO))
            {
                endereco = padeiro.DS_ENDERECO + " - " + padeiro.NM_CIDADE;
            }
            else
            {
                endereco = padeiro.NM_CIDADE;
            }

            if (!string.IsNullOrEmpty(endereco))
            {
                // Formatar o endereço para que possa ser passado na URL da API
                string enderecoFormatado = Uri.EscapeDataString(endereco);

                string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={enderecoFormatado}&key={apiKey}";

                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(url);

                    // Desserializar o JSON para um objeto
                    GoogleMapsResponse response = JsonConvert.DeserializeObject<GoogleMapsResponse>(json);

                    if (response.status == "OK")
                    {
                        // Verificar se há resultados retornados
                        if (response.results.Length > 0)
                        {
                            // Obter a latitude e longitude do primeiro resultado
                            padeiro.CD_LATITUDE = response.results[0].geometry.location.lat;
                            padeiro.CD_LONGITUDE = response.results[0].geometry.location.lng;
                        }
                        else
                        {
                            padeiro.DS_MENSAGEM = "Nenhum resultado encontrado para o endereço fornecido./n/n" + enderecoFormatado;
                        }
                    }
                    else
                    {
                        padeiro.DS_MENSAGEM = "Endereço não encontrado!";
                    }
                }
            }
            else
            {
                padeiro.DS_MENSAGEM = "Cliente não possuí endereço cadastrado!";
            }
        }

        // Classes para representar a estrutura do JSON
        public class GoogleMapsResponse
        {
            public string status { get; set; }
            public Result[] results { get; set; }
        }

        public class Result
        {
            public Geometry geometry { get; set; }
            public AddressComponent[] address_components { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
        }

        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }
}