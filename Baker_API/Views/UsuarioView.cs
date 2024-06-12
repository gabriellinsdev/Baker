using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class UsuarioView
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
        public string TP_USUARIO { get; set; }
    }
}
