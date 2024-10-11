using Microsoft.AspNetCore.Mvc;

namespace Api.Utilities
{
    public static class ErrorUtilities
    {
        public static NotFoundObjectResult FieldNotFound(string value, int id)
        {
            return new NotFoundObjectResult(new { message = $"The {value} with ID = {id} doesn't exist." });
        }

        public static NotFoundObjectResult IdPositive(int id)
        {
            return new NotFoundObjectResult(new { message = $"ID = {id} must be a positive value greater than 0." });
        }

        public static NotFoundObjectResult UniqueName(string value)
        {
            return new NotFoundObjectResult(new { message = $"The {value} name already exists. Provide a unique name." });
        }

        public static NotFoundObjectResult EmailName(string value)
        {
            return new NotFoundObjectResult(new { message = $"The {value} email already exists. Provide a unique email." });
        }

        public static string ValidateString(string value) => $"This field needs ({value}) to be a String.";

        public static string ValidateInt(string value) => $"This field needs ({value}) to be a String.";
        
        public static string MaxLengthErrorMessage(string value, int quantity) => $"The ({value}) field cannot exceed ({quantity}) characters.";

        public static string RangeValueErrorMessageInt(int range1, int range2) => $"The number must be greater than or equal to ({range1}) and less than ({range2}).";

        public static string RangeValueErrorMessageDecimal(double range1, double range2) => $"The number must be greater than or equal to ({range1}) and less than ({range2}).";

        public static string PositiveNumber(string value) => $"This field ({value}) must be a positive number.";

        public static string IsRequired(string value) => $"This field ({value}) is required.";

        public static string NoSpecialCharacters(string value) => $"The ({value}) field must not contain special characters";

        public static string NoSpecialNumbers(string value) => $"The ({value}) field must not contain numbers";
        }
}
