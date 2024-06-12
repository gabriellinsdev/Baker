namespace Baker.Application.Interfaces
{
    public interface IDashboardAppService
    {
        IEnumerable<object> GetTopProdutos(Guid id);
        IEnumerable<object> GetQuantidadeVendas(Guid id);
    }
}