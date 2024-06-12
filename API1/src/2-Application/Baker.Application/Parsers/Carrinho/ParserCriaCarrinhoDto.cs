namespace Baker.Application.Parsers.Carrinho
{
    public static class ParserCriaCarrinhoDto
    {
        public static async Task<Domain.Entities.Carrinho> Parse(Guid usuarioId)
        {
            return await Task.FromResult(new Domain.Entities.Carrinho()
            {
                CdCliente = usuarioId
            });
        }
    }
}
