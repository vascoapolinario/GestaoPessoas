using System.ComponentModel.DataAnnotations;
using TimeZoneConverter;

namespace GestaoPessoas.Validators
{
    public class IanaTimeZoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                if (value == null)
                    return ValidationResult.Success!;
                else if (value is TimeZoneInfo timeZone && TZConvert.KnownIanaTimeZoneNames.Contains(timeZone.Id))
                    return ValidationResult.Success!;
                return new ValidationResult("A propriedade timezone não é uma timezone válida de formatação Iana");
            }
            catch (Exception ex)
            {
                return new ValidationResult($"Erro ao validar a propriedade timezone: {ex.Message}");
            }
        }
    }
}