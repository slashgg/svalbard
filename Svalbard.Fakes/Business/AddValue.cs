using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Svalbard.Fakes.Business
{
  [DataContract]
  public class AddValue
  {
    [Required(ErrorMessage = "foo is required")]
    [DataMember(Name = "foo")]
    [BindProperty(Name = "foo")]
    public string FooLongerName { get; set; }
  }
}
