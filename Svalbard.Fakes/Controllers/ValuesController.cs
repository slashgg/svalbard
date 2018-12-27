using Microsoft.AspNetCore.Mvc;
using Svalbard.Fakes.Business;
using System;

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
    public OperationResult<Value> Get(int id)
    {
      return new Value { Data = "value" };
    }

    // POST api/values
    [HttpPost]
    public OperationResult<Value> Post([FromBody] AddValue value)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      return new Value { Data = value.Foo };
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public OperationResult<Value> Put(int id, [FromBody] string value)
    {
      throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public OperationResult<Value> Delete(int id)
    {
      return Unauthorized();
    }
  }
}
