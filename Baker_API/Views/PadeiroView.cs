using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class PadeiroView
    {
        [DataMember]
        public Guid CD_USUARIO { get; set; }
        [DataMember]
        public string NM_USUARIO { get; set; }
        [DataMember]
        public string DS_EMAIL { get; set; }
        [DataMember]
        public string DS_TELEFONE { get; set; }
        [DataMember]
        public string NM_ESTADO { get; set; }
        [DataMember]
        public string NM_CIDADE { get; set; }
        [DataMember]
        public string DS_ENDERECO { get; set; }
        [DataMember]
        public string CD_CEP { get; set; }
        [DataMember]
        public string CD_SENHA { get; set; }
        [DataMember]
        public string CD_CPF_CNPJ { get; set; }
        [DataMember]
        public double CD_LATITUDE { get; set; }
        [DataMember]
        public double CD_LONGITUDE { get; set; }
        [DataMember]
        public string DS_MENSAGEM { get; set; }
        [DataMember]
        public string? LS_ALIMENTOS_RESTRITOS_PADEIRO { get; set; }
        [DataMember]
        public string? LS_ALIMENTOS_RESTRITOS_PRODUTO { get; set; }

    }
}
