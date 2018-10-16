using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Svalbard.Fakes;
using Svalbard.Fakes.Business;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Svalbard.Tests
{
  public class FunctionalOperationalResultTests : IClassFixture<WebApplicationFactory<Startup>>
  {
    private readonly WebApplicationFactory<Startup> _factory;

    public FunctionalOperationalResultTests(WebApplicationFactory<Startup> factory)
    {
      _factory = factory;
    }

    [Fact]
    public async Task Get_ValuesSuccessful()
    {
      // Arrange
      var client = _factory.CreateClient();

      // Act
      var response = await client.GetAsync("/api/values");

      // Assert
      response.EnsureSuccessStatusCode();

      var data = await ReadResponse<string[]>(response);

      Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
      Assert.Collection(data, val1 => val1.Equals("value1"), val2 => val2.Equals("value2"));
    }

    [Fact]
    public async Task Post_ValuesUnsuccessful()
    {
      // Arrange
      var client = _factory.CreateClient();
      var content = JsonConvert.SerializeObject(new AddValue
      {
        Foo = ""
      });

      // Act
      var response = await client.PostAsync("/api/values", new StringContent(content, Encoding.UTF8, "application/json"));

      // Assert
      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

      var data = await ReadResponse<Error>(response);

      Assert.NotNull(data);
      Assert.Equal("Something was wrong with your input.", data.Message);
      Assert.Equal("InvalidModel", data.Code);
      Assert.Collection(data.Fields, field1 =>
      {
        Assert.Equal("foo", field1.Key);
      });
    }

    private async Task<T> ReadResponse<T>(HttpResponseMessage response)
    {
      var content = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(content);
    }
  }
}
