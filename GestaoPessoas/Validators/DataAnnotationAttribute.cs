using System.ComponentModel.DataAnnotations;
using TimeZoneConverter;

namespace GestaoPessoas.Validators
{
    internal class DataAnnotationAttribute : ValidationAttribute
    {
        public DataAnnotationAttribute()
        {
            ErrorMessage = "O timezone especificado não é válido.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;
            else if (value is TimeZoneInfo timeZone && TZConvert.KnownIanaTimeZoneNames.Contains(timeZone.Id))
                return true;
            return false;
        }
    }
}