using Baker.Application.Interfaces;
using Baker.Domain.Interfaces.Services;

namespace Baker.Application.Services
{
    public class DashboardAppService : IDashboardAppService
    {
        private readonly IDashboardService _dashboardService;

        public DashboardAppService(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public IEnumerable<object> GetTopProdutos(Guid id)
        {
            IEnumerable<object> retorno = _dashboardService.GetTopProdutos(id);
            if (retorno is null || !retorno.Any()) throw new ArgumentNullException();
            return retorno;
        }

        public IEnumerable<object> GetQuantidadeVendas(Guid id)
        {
            IEnumerable<object> retorno = _dashboardService.GetQuantidadeVendas(id);
            if (retorno is null || !retorno.Any()) throw new ArgumentNullException();
            return retorno;
        }
    }
}
