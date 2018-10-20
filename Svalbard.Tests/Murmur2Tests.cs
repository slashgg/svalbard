using Svalbard.Utils;
using Xunit;

namespace Svalbard.Tests
{
  public class Murmur2Tests
  {
    [Fact]
    public void HashTest() {
      var result = MurmurHash2.ComputeHash("test@email.com");
      Assert.Equal(2205790562, result);
    }
  }
}