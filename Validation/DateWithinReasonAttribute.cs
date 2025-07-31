using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Validation
{
    public class DateWithinReasonAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                var today = DateTime.Today;
                if (date < today.AddYears(-1) || date > today)
                {
                    return new ValidationResult("The date must be within one year from today.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
