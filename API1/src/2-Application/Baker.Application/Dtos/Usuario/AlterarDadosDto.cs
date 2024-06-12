using System.ComponentModel.DataAnnotations;

namespace Baker.Application.Dtos.Usuario
{
    public class AlterarDadosDto
    {
        [MaxLength(100)]
        public string? NomeUsuario { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(11)]
        public string? Telefone { get; set; }

        [MaxLength(50)]
        public string? Estado { get; set; }

        [MaxLength(50)]
        public string? Cidade { get; set; }

        [MaxLength(100)]
        public string? Endereco { get; set; }

        [MaxLength(8)]
        public string? Cep { get; set; }

        [MaxLength(60)]
        [Required(ErrorMessage = "O campo \"Senha Antiga\" é obrigatório.")]
        public string SenhaAntiga { get; set; } = null!;

        [MaxLength(60)]
        public string? SenhaNova { get; set; }
    }
}
