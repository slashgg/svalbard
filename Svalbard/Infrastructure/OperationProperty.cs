using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Svalbard.Infrastructure
{
  /// <summary>
  /// Mark this property as a property we want to track with Svalbard validators and binding conventions.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class OperationProperty : ValidationAttribute, IModelNameProvider
  {
    /// <summary>
    /// Model property name
    /// </summary>
    public string Name { get; }

    public OperationProperty(string name)
    {
      Name = name;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var validators = validationContext.ObjectType.GetProperty(validationContext.MemberName).GetCustomAttributes(typeof(IValidate), true);
      foreach (IValidate validator in validators)
      {
        if (validator.IsInvalid(value))
        {
          return new ValidationResult(ErrorMessage ?? validator.GetErrorMessage());
        }
      }

      return ValidationResult.Success;
    }
  }
}
