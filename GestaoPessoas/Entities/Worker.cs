using GestaoPessoas.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoPessoas.Entities
{
    [Table("Workers")]
    public class Worker
    {
        /// <summary>
        /// Identificador do trabalhador nos dados.
        /// Atribuido pelo sistema, tipo int, exemplo: 1
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do trabalhador, string com maximo de 100 caracteres, exemplo: Alexandre Silva
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Titulo do emprego do trabalhador, string com maximo de 100 caracteres, exemplo: Programador
        /// </summary>
        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// Email do trabalhador, string com maximo de 100 caracteres, necessita de representar um email, exemplo: example@gmail.com
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do trabalhador. Aceita datas entre 150 anos atrás e a data atual.
        /// </summary>
        [Required]
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// Timezone do trabalhador. Necessita de ser um timezone válido. Por omissão é UTC.
        /// </summary>
        [Required]
        public string TimeZone { get; set; } = TimeZoneInfo.Utc.Id;


        /// <summary>
        /// Username do utilzador no Portal das Finanças, string com maximo de 100 caracteres, exemplo:
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PortalDasFinancasUserName { get; set; } = string.Empty;

        /// <summary>
        /// Password do utilzador no Portal das Finanças, string.
        /// </summary>
        public string? PortalDasFinancasUserPassword { get; set; } = null;
    }
}
