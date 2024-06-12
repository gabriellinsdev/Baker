using System.ComponentModel.DataAnnotations;

namespace Baker.Application.Dtos.Usuario
{
    public class CadastrarDto
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
        public string Nome { get; set; } = null!;

        [MaxLength(100)]
        [Required(ErrorMessage = "O campo \"Email\" é obrigatório.")]
        public string Email { get; set; } = null!;

        [MaxLength(11)]
        [Required(ErrorMessage = "O campo \"Telefone\" é obrigatório.")]
        public string Telefone { get; set; } = null!;

        [MaxLength(50)]
        [Required(ErrorMessage = "O campo \"Estado\" é obrigatório.")]
        public string Estado { get; set; } = null!;

        [MaxLength(50)]
        [Required(ErrorMessage = "O campo \"Cidade\" é obrigatório.")]
        public string Cidade { get; set; } = null!;

        [MaxLength(100)]
        [Required(ErrorMessage = "O campo \"Endereço\" é obrigatório.")]
        public string Endereco { get; set; } = null!;

        [MaxLength(8)]
        [Required(ErrorMessage = "O campo \"Cep\" é obrigatório.")]
        public string Cep { get; set; } = null!;

        [MaxLength(60)]
        [Required(ErrorMessage = "O campo \"Senha\" é obrigatório.")]
        public string Senha { get; set; } = null!;

        [MaxLength(14)]
        [Required(ErrorMessage = "O campo \"Cpf/Cnpj\" é obrigatório.")]
        public string CpfCnpj { get; set; } = null!;
    }
}
