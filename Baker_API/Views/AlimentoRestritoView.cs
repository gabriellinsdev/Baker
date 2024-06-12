using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class AlimentoRestritoView
    {
        [DataMember]
        public int? CD_ALIMENTO_RESTRITO {  get; set; }
        [DataMember]
        public string? DS_ALIMENTO_RESTRITO { get; set; }
    }
}
