using Svalbard.Infrastructure;

namespace Svalbard.Fakes.Business
{
  public class AddValue
  {
    [Required]
    [OperationProperty("foo")]
    public string Foo { get; set; }
  }
}
