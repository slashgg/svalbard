using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Svalbard.Fakes.Business;

namespace Svalbard.Fakes.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    // GET api/values
    [HttpGet]
    public OperationResult<string[]> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public OperationResult<string> Get(int id)
    {
      return "value";
    }

    // POST api/values
    [HttpPost]
    public OperationResult<bool> Post([FromBody] AddValue value)
    {
      if (ModelState.IsValid)
      {
        return true;
      }

      return false;
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
