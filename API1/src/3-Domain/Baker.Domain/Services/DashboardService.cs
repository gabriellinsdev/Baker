using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;

namespace Baker.Domain.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public IEnumerable<object> GetTopProdutos(Guid id)
        {
            return _dashboardRepository.GetTopProdutos(id);
        }

        public IEnumerable<object> GetQuantidadeVendas(Guid id)
        {
            return _dashboardRepository.GetQuantidadeVendas(id);
        }
    }
}
