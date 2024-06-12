using Baker.Application.Interfaces;
using Baker.Application.Services;
using Baker.Domain.Interfaces.Repositories;
using Baker.Domain.Interfaces.Services;
using Baker.Domain.Interfaces.UoW;
using Baker.Domain.Services;
using Baker.Infra.Data.Context;
using Baker.Infra.Data.Repositories;
using Baker.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Baker.Infra.IoC
{
    public static class ServiceExtensions
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            //Configuration
            services.TryAddSingleton(config);

            //Application Services
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IItemCarrinhoAppService, ItemCarrinhoAppService>();
            services.AddScoped<IPedidoAppService, PedidoAppService>();
            services.AddScoped<IDashboardAppService, DashboardAppService>();

            //Domain Services
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICarrinhoService, CarrinhoService>();
            services.AddScoped<IItemCarrinhoService, ItemCarrinhoService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IItemPedidoService, ItemPedidoService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IDashboardService, DashboardService>();

            //Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
            services.AddScoped<IItemCarrinhoRepository, ItemCarrinhoRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<BakerDbContext>(x =>
            {
                x.UseSqlServer(config.GetConnectionString("BakerMSSQLS"));
                x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}