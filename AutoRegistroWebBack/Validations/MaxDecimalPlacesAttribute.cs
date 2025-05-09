
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AutoRegistroWebBack.Validations
{
    public class MaxDecimalPlacesAttribute : ValidationAttribute
    {
        private readonly int _decimalPlaces;

        public MaxDecimalPlacesAttribute(int decimalPlaces)
        {
            _decimalPlaces = decimalPlaces;
            ErrorMessage = $"O campo deve ter no máximo {_decimalPlaces} casas decimais.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (decimal.TryParse(Convert.ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
            {
                var partes = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');
                if (partes.Length == 2 && partes[1].Length > _decimalPlaces)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
