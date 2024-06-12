using System.Runtime.Serialization;

namespace Baker_API.Models
{
    public class UsuarioModel
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
        public string TP_USUARIO { get; set; }
    }
}
