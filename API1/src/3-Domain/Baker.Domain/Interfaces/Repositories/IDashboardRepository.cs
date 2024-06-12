namespace Baker.Domain.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        IEnumerable<object> GetTopProdutos(Guid id);

        IEnumerable<object> GetQuantidadeVendas(Guid id);
    }
}
