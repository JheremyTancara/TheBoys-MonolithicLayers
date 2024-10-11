using System.ComponentModel.DataAnnotations;
using Api.Utilities;

public class MaxLengthCharacters : ValidationAttribute
{
    private readonly int _maxLength;

    public MaxLengthCharacters(string value, int maxLength)
    {
        _maxLength = maxLength;
        ErrorMessage = ErrorUtilities.MaxLengthErrorMessage(value, maxLength);
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            if (stringValue.Length > _maxLength)
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success!;
    }
}
