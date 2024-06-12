using System.ComponentModel.DataAnnotations;

namespace Baker.Application.Dtos.Usuario
{
    public class LogarDto
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "O campo \"Email\" é obrigatório.")]
        public string Email { get; set; } = null!;

        [MaxLength(60)]
        [Required(ErrorMessage = "O campo \"Senha\" é obrigatório.")]
        public string Senha { get; set; } = null!;
    }
}
