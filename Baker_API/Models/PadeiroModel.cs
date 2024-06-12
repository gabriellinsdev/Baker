using System.Runtime.Serialization;

namespace Baker_API.Models
{
    public class PadeiroModel
    {
        public Guid CD_USUARIO { get; set; }
        public string NM_USUARIO { get; set; }
        public string DS_EMAIL { get; set; }
        public string DS_TELEFONE { get; set; }
        public string NM_ESTADO { get; set; }
        public string NM_CIDADE { get; set; }
        public string DS_ENDERECO { get; set; }
        public string CD_CEP { get; set; }
        public string CD_SENHA { get; set; }
        public string CD_CPF_CNPJ { get; set; }
        public double CD_LATITUDE { get; set; }
        public double CD_LONGITUDE { get; set; }
        public string DS_MENSAGEM { get; set; }
        public string? LS_ALIMENTOS_RESTRITOS_PADEIRO { get; set; }
        public string? LS_ALIMENTOS_RESTRITOS_PRODUTO { get; set; }
    }
}
