using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Api.Utilities;

public class NoNumbers : ValidationAttribute
{
    public NoNumbers(string value)
    {
        ErrorMessage = ErrorUtilities.NoSpecialNumbers(value);
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            var regex = new Regex("^[a-zA-Z ]*$");

            if (!regex.IsMatch(stringValue))
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success!;
    }
}
