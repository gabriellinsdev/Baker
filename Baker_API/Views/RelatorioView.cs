using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class RelatorioView
    {
        [DataMember]
        public Guid CD_PEDIDO { get; set; }
        [DataMember]
        public string NM_PADEIRO { get; set; }
        [DataMember]
        public string NM_CLIENTE { get; set; }
        [DataMember]
        public DateTime DT_PEDIDO { get; set; }
        [DataMember]
        public string NM_PRODUTO { get; set; }
        [DataMember]
        public int QT_PRODUTO { get; set; }
        [DataMember]
        public decimal VL_PRECO { get; set; }
        [DataMember]
        public decimal VL_SUBTOTAL { get; set; }
        [DataMember]
        public decimal VL_TOTAL { get; set; }
        [DataMember]
        public string NM_ESTADO { get; set; }
        [DataMember]
        public string NM_CIDADE { get; set; }
        [DataMember]
        public string DS_ENDERECO { get; set; }
    }
}
