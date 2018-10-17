using System;

namespace Svalbard.Infrastructure
{
  /// <summary>
  /// Marks this property as required for a valid model.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class Required : Attribute, IValidate
  {
    public bool IsInvalid(object value)
    {
      if (value == null)
      {
        return true;
      }

      // If this is a value type, it will have a value and therefore will pass.
      if (value.GetType().IsValueType)
      {
        return false;
      }

      // For strings we want to ensure the string isn't empty.
      return value is string && string.IsNullOrEmpty((string)value);
    }

    public string GetErrorMessage()
    {
      return "This field is required.";
    }
  }
}
