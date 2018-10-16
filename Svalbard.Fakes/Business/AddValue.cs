using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Svalbard.Fakes.Business
{
  [DataContract]
  public class AddValue
  {
    [Required]
    [DataMember(Name = "foo")]
    public string Foo { get; set; }
  }
}
