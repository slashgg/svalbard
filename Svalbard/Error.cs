using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Svalbard
{
  /// <summary>
  /// Error container for 400-500 errors.
  /// </summary>
  public class Error
  {
    public string Message { get; set; }
    public string Code { get; set; }
    public ICollection<FieldError> Fields { get; set; } = new HashSet<FieldError>();
  }

  public class FieldError
  {
    public string Key { get; set; }
    public string AttemptedValue { get; set; }
    public IEnumerable<string> Messages { get; set; }

    public FieldError()
    {
    }

    public FieldError(string key, string attemptedValue, IEnumerable<string> messages)
    {
      this.Key = key;
      this.AttemptedValue = attemptedValue;
      this.Messages = messages;
    }
  }

  public class ServiceError
  {
    public string Key { get; set; }
    public int StatusCode { get; set; }

    public ServiceError() { }
    public ServiceError(string key, int statusCode)
    {
      this.Key = key;
      this.StatusCode = statusCode;
    }
  }
}
