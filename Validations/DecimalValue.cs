using System.ComponentModel.DataAnnotations;

public class DecimalValue : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not null)
        {
            if (decimal.TryParse(value.ToString(), out _))
            {
                return ValidationResult.Success!;
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? "The value must be a decimal number.");
            }
        }

        return ValidationResult.Success!;
    }
}
