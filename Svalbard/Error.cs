using System.Collections.Generic;

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
  }
}
