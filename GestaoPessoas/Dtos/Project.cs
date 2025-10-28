using System.ComponentModel.DataAnnotations;

namespace GestaoPessoas.Dtos
{
    public class Project
    {
        /// <summary>
        /// Identificador do projeto nos dados.
        /// Atribuido pelo sistema, tipo int, exemplo: 1
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Nome do projeto, string com maximo de 100 caracteres, exemplo: GestaoPessoas
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Lista de nomes dos trabalhadores associados ao projeto.
        /// </summary>
        [Required]
        public List<string> Workers { get; set; } = new List<string>();

        /// <summary>
        /// Quantidade de trabalhadores associados ao projeto. Tipo: int
        /// </summary>
        [Required]
        public int WorkersCount = 0;
    }
}
