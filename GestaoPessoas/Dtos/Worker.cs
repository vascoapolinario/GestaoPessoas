using GestaoPessoas.Validators;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;

namespace GestaoPessoas.Dtos
{
    public class Worker : IValidatableObject
    {
        /// <summary>
        /// Identificador do trabalhador nos dados.
        /// Atribuido pelo sistema, tipo int, exemplo: 1
        /// </summary>
        [Required]
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
        [EmailAddress]
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
        [IanaTimeZone]
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;

        public override bool Equals(object? obj)
        {
            if (obj is not Worker other)
                return false;
            return Id == other.Id &&
                   Name == other.Name &&
                   JobTitle == other.JobTitle &&
                   Email == other.Email &&
                   BirthDate == other.BirthDate &&
                   TimeZone.Id == other.TimeZone.Id;
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (BirthDate > DateOnly.FromDateTime(DateTime.Now))
            {
                yield return new ValidationResult(
                    "BirthDate cannot be in the future.",
                    new[] { nameof(BirthDate) });
            }
            else if (BirthDate < DateOnly.FromDateTime(DateTime.Now.AddYears(-150)))
            {
                yield return new ValidationResult(
                    "BirthDate is not realistic.",
                    new[] { nameof(BirthDate) });
            }
        }
    }
}
