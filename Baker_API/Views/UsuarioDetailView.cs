using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class UsuarioDetailView
    {
        [DataMember]
        public Guid CD_USUARIO { get; set; }
        [DataMember]
        public string NM_USUARIO { get; set; }
        [DataMember]
        public string TP_USUARIO { get; set; }
    }
}
