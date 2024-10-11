using System.ComponentModel.DataAnnotations;

namespace Api.Models

{
      public class DecimalPrecision : ValidationAttribute
    {
        private readonly int _precision;

        public DecimalPrecision(int precision)
        {
            _precision = precision;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && value is decimal decimalValue)
            {
                var scale = BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];
                if (scale > _precision)
                {
                    return new ValidationResult($"The '{validationContext.DisplayName}' property must have up to {_precision} decimal places.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
