using System.ComponentModel.DataAnnotations;

namespace IdentityServer.DTO
{
    public class DateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value is string s && DateOnly.TryParse(s, out _))
                return ValidationResult.Success;
            else
                return new ValidationResult($"{value} was not recognized as a valid date.");
        }
    }
}
