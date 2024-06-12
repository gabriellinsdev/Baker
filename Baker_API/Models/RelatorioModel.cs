namespace Baker_API.Models
{
    public class RelatorioModel
    {
        public Guid CD_PEDIDO { get; set; }
        public string NM_PADEIRO { get; set; }
        public string NM_CLIENTE { get; set; }
        public DateTime DT_PEDIDO { get; set; }
        public string NM_PRODUTO { get; set; }
        public int QT_PRODUTO { get; set; }
        public decimal VL_PRECO { get; set; }
        public decimal VL_SUBTOTAL { get; set; }
        public decimal VL_TOTAL { get; set; }
        public string NM_ESTADO { get; set; }
        public string NM_CIDADE { get; set; }
        public string DS_ENDERECO { get; set; }
    }
}
