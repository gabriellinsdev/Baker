using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class CarrinhoView
    {
        [DataMember]
        public Guid? CD_USUARIO { get; set; }
        [DataMember]
        public int CD_PRODUTO { get; set; }
        [DataMember]
        public int QT_PRODUTO { get; set; }
        [DataMember]
        public decimal VL_PRECO{ get; set; }
        [DataMember]
        public byte[]? VB_IMAGEM { get; set; }
    }
}
