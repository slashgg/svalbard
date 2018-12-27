using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Svalbard.Fakes;
using Svalbard.Fakes.Business;
using System.Linq;
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
    public async Task Get_ValueSuccessful()
    {
      // Arrange
      var client = _factory.CreateClient();

      // Act
      var response = await client.GetAsync("/api/values/1");

      // Assert
      response.EnsureSuccessStatusCode();

      var data = await ReadResponse<Value>(response);

      Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
      Assert.True(response.Content.Headers.ContentLength > 0);
    }

    [Fact]
    public async Task Post_Value()
    {
      // Arrange
      var client = _factory.CreateClient();
      var content = JsonConvert.SerializeObject(new AddValue
      {
        Foo = "data"
      });

      // Act
      var response = await client.PostAsync("/api/values", new StringContent(content, Encoding.UTF8, "application/json"));

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      var data = await ReadResponse<Value>(response);

      Assert.Equal("data", data.Data);
    }

    [Fact]
    public async Task Post_ValuesBadRequest()
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
        Assert.Equal("This field is required.", field1.Messages.First());
        Assert.Null(field1.AttemptedValue);
      });
    }

    [Fact]
    public async Task Put_ValuesServerError()
    {
      // Arrange
      var client = _factory.CreateClient();
      var content = JsonConvert.SerializeObject(new { value = "data" });

      // Act
      var response = await client.PutAsync("/api/values/1", new StringContent(content, Encoding.UTF8, "application/json"));

      // Assert
      Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

      var data = await ReadResponse<Error>(response);

      Assert.Equal("The method or operation is not implemented.", data.Message);
      Assert.Equal("ServerError", data.Code);
      Assert.Empty(data.Fields);
    }

    [Fact]
    public async Task Delete_ValuesUnauthorized()
    {
      // Arrange
      var client = _factory.CreateClient();
      var content = JsonConvert.SerializeObject(new { value = "data" });

      // Act
      var response = await client.DeleteAsync("/api/values/1");

      // Assert
      Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
      Assert.Equal(0, response.Content.Headers.ContentLength);
    }

    private async Task<T> ReadResponse<T>(HttpResponseMessage response)
    {
      var content = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(content);
    }
  }
}
