using Baker.Application.Dtos.Usuario;

namespace Baker.Application.Parsers.Usuario
{
    public static class ParserAlterarDadosDto
    {
        public static async Task<Domain.Entities.Usuario> Parse(AlterarDadosDto item, Domain.Entities.Usuario usuario)
        {
            return await Task.FromResult(new Domain.Entities.Usuario()
            {
                CdUsuario = usuario.CdUsuario,
                NmUsuario = item.NomeUsuario is null ? usuario.NmUsuario : item.NomeUsuario,
                DsEmail = item.Email is null ? usuario.DsEmail : item.Email,
                DsTelefone = item.Telefone is null ? usuario.DsTelefone : item.Telefone,
                NmEstado = item.Estado is null ? usuario.NmEstado : item.Estado,
                NmCidade = item.Cidade is null ? usuario.NmCidade : item.Cidade,
                DsEndereco = item.Endereco is null ? usuario.DsEndereco : item.Endereco,
                CdCep = item.Cep is null ? usuario.CdCep : item.Cep,
                CdSenha = item.SenhaNova is null ? usuario.CdSenha : item.SenhaNova,
                CdCpfCnpj = usuario.CdCpfCnpj
            });
        }
    }
}
