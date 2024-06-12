using Baker.Application.Dtos.Pedido;
using Baker.Domain.Entities;

namespace Baker.Application.Interfaces
{
    public interface IPedidoAppService
    {
        Task<IEnumerable<GetPedidosDto>> GetPedidosByUsuarioId(Guid id);

        Task<GetPedidosDto> GetPedido(Guid id);

        Task<CriarPedidoDtoResponse> CriaPedido(CriarPedidoDtoRequest request);

        Task EnviaEmail(Pedido pedido, string emailPadeiro, string emailCliente, string nmPadeiro, string nmCliente, IEnumerable<ItemPedidoDto> itens);
    }
}