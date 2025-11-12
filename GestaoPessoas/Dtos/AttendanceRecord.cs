using System.ComponentModel.DataAnnotations;
using GestaoPessoas.Enums;

namespace GestaoPessoas.Dtos
{
    public class AttendanceRecord
    {
        /// <summary>
        /// Identificador do registo de assiduidade nos dados.
        /// Atribuido pelo sistema, tipo int, exemplo: 1
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Identificador do trabalhador nos dados.
        /// Atribuido pelo sistema, tipo int, exemplo: 1
        /// </summary>
        [Required]
        public int WorkerId { get; set; }

        /// <summary>
        /// Nome do trabalhador, string com maximo de 100 caracteres, exemplo: Alexandre Silva
        /// </summary>
        [Required]
        [StringLength(100)]
        public string WorkerName { get; set; } = string.Empty;


        /// <summary>
        /// Tipo de evento de assiduidade. Pode ser "CheckIn", "CheckOut" ou "Undefined". Caso não seja especificado ou o valor dado não seja um destes valores, o valor padrão é "Undefined".
        /// </summary>
        [Required]
        public string EventType { get; set; } = "Undefined";

        /// <summary>
        /// Data e hora do evento de assiduidade. Tipo DateTimeOffset, exemplo: 2023-01-15T08:30:00+00:00
        /// Em UTC.
        /// </summary>
        [Required]
        public DateTimeOffset EventDate { get; set; }
    }
}
