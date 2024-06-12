using Baker.Application.Dtos.Usuario;

namespace Baker.Application.Parsers.Usuario
{
    public static class ParserGetDadosDto
    {
        public static async Task<GetUsuarioDto> Parse(Domain.Entities.Usuario item)
        {
            return await Task.FromResult(new GetUsuarioDto()
            {
                Nome = item.NmUsuario,
                Email = item.DsEmail,
                Telefone = item.DsTelefone,
                Estado = item.NmEstado,
                Cidade = item.NmCidade,
                Endereco = item.DsEndereco,
                Cep = item.CdCep,
                CpfCnpj = item.CdCpfCnpj
            });
        }
    }
}
