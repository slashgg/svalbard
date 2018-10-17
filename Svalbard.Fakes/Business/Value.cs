using System.Runtime.Serialization;

namespace Svalbard.Fakes.Business
{
  [DataContract]
  public class Value
  {
    [DataMember(Name = "data")]
    public string Data { get; set; }

    public static Value Empty = new Value();
  }
}
