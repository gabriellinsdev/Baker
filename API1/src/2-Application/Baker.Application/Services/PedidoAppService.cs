using Baker.Application.Dtos.Pedido;
using Baker.Application.Interfaces;
using Baker.Application.Parsers.Pedido;
using Baker.Domain.Entities;
using Baker.Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Concurrent;
using System.Text;

namespace Baker.Application.Services
{
    public class PedidoAppService : IPedidoAppService
    {
        private readonly IPedidoService _pedidoService;
        private readonly ICarrinhoService _carrinhoService;
        private readonly IItemCarrinhoService _itemCarrinhoService;
        private readonly IProdutoService _produtoService;
        private readonly IItemPedidoService _itemPedidoService;
        private readonly IUsuarioService _usuarioService;

        public PedidoAppService(IPedidoService pedidoService, ICarrinhoService carrinhoService, IItemCarrinhoService itemCarrinhoService, IProdutoService produtoService, IItemPedidoService itemPedidoService, IUsuarioService usuarioService)
        {
            _pedidoService = pedidoService;
            _carrinhoService = carrinhoService;
            _itemCarrinhoService = itemCarrinhoService;
            _produtoService = produtoService;
            _itemPedidoService = itemPedidoService;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<GetPedidosDto>> GetPedidosByUsuarioId(Guid id)
        {
            IEnumerable<Pedido> retorno = await _pedidoService.GetPedidosByUsuarioId(id);
            if (retorno is null || !retorno.Any()) throw new ArgumentNullException();
            ConcurrentBag<GetPedidosDto> itemsRetorno = new();
            foreach (var item in retorno)
            {
                IEnumerable<ItemPedido> itensPedido = await _itemPedidoService.GetItemPedidoByPedidoId(item.CdPedido);

                ConcurrentBag<ItemPedidoDto> itemsRetorno2 = new();
                foreach (var item2 in itensPedido)
                {
                    Produto produto = await _produtoService.GetProdutoById(item2.CdProduto);
                    itemsRetorno2.Add(await ParserItensCriarPedidoDtoResponse.Parse(item2, produto.NmProduto));
                }

                Usuario padeiro = await _usuarioService.GetUsuarioById(item.CdPadeiro);
                Usuario cliente = await _usuarioService.GetUsuarioById(item.CdCliente);
                itemsRetorno.Add(await ParserGetPedidosDto.Parse(item, padeiro, cliente, itemsRetorno2));
            }
            return itemsRetorno;
        }

        public async Task<GetPedidosDto> GetPedido(Guid id)
        {
            Pedido retorno = await _pedidoService.GetPedidoById(id);
            if (retorno is null) throw new ArgumentNullException();
            Usuario padeiro = await _usuarioService.GetUsuarioById(retorno.CdPadeiro);
            Usuario cliente = await _usuarioService.GetUsuarioById(retorno.CdCliente);
            IEnumerable<ItemPedido> itensPedido = await _itemPedidoService.GetItemPedidoByPedidoId(retorno.CdPedido);

            ConcurrentBag<ItemPedidoDto> itemsRetorno = new();
            foreach (var item in itensPedido)
            {
                Produto produto = await _produtoService.GetProdutoById(item.CdProduto);
                itemsRetorno.Add(await ParserItensCriarPedidoDtoResponse.Parse(item, produto.NmProduto));
            }

            return await ParserGetPedidosDto.Parse(retorno, padeiro, cliente, itemsRetorno);
        }

        public async Task<CriarPedidoDtoResponse> CriaPedido(CriarPedidoDtoRequest request)
        {
            Carrinho carrinho = await _carrinhoService.GetCarrinhoByUsuarioId(request.CodigoCliente);
            IEnumerable<ItemCarrinho> itensCarrinho = await _itemCarrinhoService.GetItemCarrinhoByCarrinhoId(carrinho.CdCarrinho);
            itensCarrinho = itensCarrinho.Where(x => request.CodigoItemDoCarrinho.Contains(x.CdItemDoCarrinho));
            Produto produto = await _produtoService.GetProdutoById(itensCarrinho.First().CdProduto);
            Pedido pedido = await ParserCriarPedidoRequestDto.Parse(request, itensCarrinho, produto);
            await _pedidoService.CriaPedido(pedido);
            await _itemPedidoService.InsereItens(await ParserInserirItemPedidoDto.Parse(itensCarrinho, pedido));
            await _itemCarrinhoService.DeletaItens(itensCarrinho);
            Usuario cliente = await _usuarioService.GetUsuarioById(pedido.CdCliente);
            Usuario padeiro = await _usuarioService.GetUsuarioById(pedido.CdPadeiro);

            ConcurrentBag<ItemPedidoDto> itemsRetorno = new();
            foreach (var item in itensCarrinho)
            {
                produto = await _produtoService.GetProdutoById(item.CdProduto);
                itemsRetorno.Add(await ParserItensCriarPedidoDtoResponse.Parse(item, produto.NmProduto));
            }

            await EnviaEmail(pedido, padeiro.DsEmail, cliente.DsEmail, padeiro.NmUsuario, cliente.NmUsuario, itemsRetorno);

            return await ParserCriarPedidoDtoResponse.Parse(pedido, itemsRetorno, cliente, padeiro);
        }

        public async Task EnviaEmail(Pedido pedido, string emailPadeiro, string emailCliente, string nmPadeiro, string nmCliente, IEnumerable<ItemPedidoDto> itens)
        {
            StringBuilder listaItens = new StringBuilder();
            int contador = 1;
            foreach (var item in itens)
            {
                listaItens.Append($"<li>Item {contador}</li>");
                listaItens.Append("<ul>");
                listaItens.Append($"<li>Nome: {item.NomeProduto}</li>");
                listaItens.Append($"<li>Quantidade: {item.QuantidadeProduto}</li>");
                listaItens.Append($"<li>Valor: {item.Valor}</li>");
                listaItens.Append("</ul>");
                contador++;
            }

            var mailboxAdressess = new List<MailboxAddress> { new("", emailPadeiro), new("", emailCliente), new("", "raigreys@gmail.com"), new("", "leemoslucio@gmail.com") };

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", "sitebakertcc@gmail.com"));
            emailMessage.To.AddRange(mailboxAdressess);
            emailMessage.Subject = $"Pedido nº {pedido.CdPedido}";

            string htmlBody = $@"
                                <!DOCTYPE html>
                                <html lang=""pt-br"">
                                <head>
                                    <meta charset=""UTF-8"">
                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                </head>
                                <body>
                                    <h1>Você recebeu um novo pedido via site Baker!</h1>
                                    <p>Detalhes do pedido: </p>
                                    <ul>
                                        <li>Codigo do Pedido: {pedido.CdPedido}</li>
                                        <li>Nome do Padeiro: {nmPadeiro}</li>
                                        <li>Nome do Cliente: {nmCliente}</li>
                                        <li>Data: {pedido.DtPedido}</li>
                                        <li>Valor: {pedido.VlTotal}</li>
                                        <li>Estado: {pedido.NmEstado}</li>
                                        <li>Cidade: {pedido.NmCidade}</li>
                                        <li>Endereco: {pedido.DsEndereco}</li>
                                        <li>Cep: {pedido.CdCep}</li>
                                        <li>Observacao: {pedido.DsObservacao}</li>
                                        <li>Itens:</li>
                                            <ul>   
                                                {listaItens}
                                            </ul>
                                    </ul>
                                </body>
                                </html>
                            ";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlBody;

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            client.Connect("smtp.gmail.com", 465, true);
            client.AuthenticationMechanisms.Remove("XOUATH2");
            client.Authenticate("sitebakertcc@gmail.com", "makx llme pxsf brrk");

            await client.SendAsync(emailMessage);
        }
    }
}
