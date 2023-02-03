using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.Attributes;

public class CheckModelSalaryAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public CheckModelSalaryAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ErrorMessage = ErrorMessageString;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        var currentValue = Convert.ToInt32(value);
        
        if (property is null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = Convert.ToInt32(property.GetValue(validationContext.ObjectInstance));

        if (currentValue > comparisonValue)
            return new ValidationResult(ErrorMessage);
        
        return ValidationResult.Success;
    }
}
