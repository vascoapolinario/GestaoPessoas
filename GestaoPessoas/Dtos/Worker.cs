using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;

namespace GestaoPessoas.Dtos
{
    public class Worker : IValidatableObject
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? JobTitle { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        public DateTimeOffset?  DAtaUltimoProcessamentoSalarial { get; set; }

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
