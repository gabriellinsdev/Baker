using Baker.Application.Dtos.Usuario;

namespace Baker.Application.Parsers.Usuario
{
    public static class ParserCadastrarDto
    {
        public static async Task<Domain.Entities.Usuario> Parse(CadastrarDto item)
        {
            return await Task.FromResult(new Domain.Entities.Usuario()
            {

                CdUsuario = Guid.NewGuid(),
                NmUsuario = item.Nome,
                DsEmail = item.Email,
                DsTelefone = item.Telefone,
                NmEstado = item.Estado,
                NmCidade = item.Cidade,
                DsEndereco = item.Endereco,
                CdCep = item.Cep,
                CdSenha = item.Senha,
                CdCpfCnpj = item.CpfCnpj
            });
        }
    }
}
