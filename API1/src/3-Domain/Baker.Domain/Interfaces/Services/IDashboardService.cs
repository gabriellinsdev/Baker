namespace Baker.Domain.Interfaces.Services
{
    public interface IDashboardService
    {
        IEnumerable<object> GetTopProdutos(Guid id);

        IEnumerable<object> GetQuantidadeVendas(Guid id);
    }
}