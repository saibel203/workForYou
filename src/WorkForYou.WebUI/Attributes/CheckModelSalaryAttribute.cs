﻿using System.ComponentModel.DataAnnotations;

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

        if (value is not int)
            return new ValidationResult(ErrorMessage = "Значення повинно бути числом");
        
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        var currentValue = Convert.ToInt32(value);

        if (property is null)
            throw new ArgumentException("Property with this name not found");
        
        if (property.GetValue(validationContext.ObjectInstance) is not int)
            return new ValidationResult(ErrorMessage = "Значення повинно бути числом");
        
        var comparisonValue = Convert.ToInt32(property.GetValue(validationContext.ObjectInstance));

        if (currentValue > comparisonValue)
            return new ValidationResult(ErrorMessage);
        
        return ValidationResult.Success;
    }
}
