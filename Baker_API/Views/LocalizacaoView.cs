using System;
using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class LocalizacaoView
    {
        [DataMember]
        public Guid? CD_USUARIO { get; set; }
        [DataMember]
        public string? NM_USUARIO { get; set; }
        [DataMember]
        public string? NM_CIDADE { get; set; }
        [DataMember]
        public double CD_LATITUDE { get; set; }
        [DataMember]
        public double CD_LONGITUDE { get; set; }
        [DataMember]
        public string? DS_MENSAGEM { get; set; }
        [DataMember]
        public bool GLUTEN { get; set; }
        [DataMember]
        public bool LACTOSE { get; set; }
        [DataMember]
        public bool LOW_CARB { get; set; }
        [DataMember]
        public bool ARTESANAL { get; set; }
    }
}
