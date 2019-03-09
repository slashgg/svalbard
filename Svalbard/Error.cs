using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Svalbard
{
  /// <summary>
  /// Error container for 400-500 errors.
  /// </summary>
  [DataContract]
  public class Error
  {
    [DataMember(Name = "message")]
    public string Message { get; set; }
    [DataMember(Name = "code")]
    public string Code { get; set; }
    [DataMember(Name = "fields")]
    public ICollection<FieldError> Fields { get; set; } = new HashSet<FieldError>();

    public Error()
    {
    }

    public Error(ServiceError error)
    {
      this.Code = error.StatusCode.ToString();
      this.Message = error.Key;
    }

    public Error(string message, string code)
    {
      this.Code = code;
      this.Message = message;
    }
  }

  [DataContract]
  public class FieldError
  {
    [DataMember(Name = "key")]
    public string Key { get; set; }
    [DataMember(Name = "attemptedValue")]
    public string AttemptedValue { get; set; }
    [DataMember(Name = "messages")]
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

    public FieldError(string key, string attemptedValue, string message)
    {
      this.Key = key;
      this.AttemptedValue = attemptedValue;
      this.Messages = new List<string> {message};
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
