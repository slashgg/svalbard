using Microsoft.AspNetCore.Mvc;
using Svalbard.Fakes.Business;
using Xunit;

namespace Svalbard.Tests
{
  public class OperationalResultTests
  {
    [Fact]
    public void ImplicitOperatorTests()
    {
      // Arrange
      var result = new OperationResult<Value>();

      // Act
      OperationResult<Value> fromBadRequest = new BadRequestResult();
      OperationResult<Value> fromConflict = new ConflictResult();
      OperationResult<Value> fromNoContent = new NoContentResult();
      OperationResult<Value> fromNotFound = new NotFoundResult();
      OperationResult<Value> fromOk = new OkResult();
      OperationResult<Value> fromUnauthorized = new UnauthorizedResult();
      OperationResult<Value> fromUnprocessableEntity = new UnprocessableEntityResult();
      OperationResult<Value> unsupportedMediaType = new UnprocessableEntityResult();

      Assert.NotNull(fromBadRequest);
      Assert.NotNull(fromConflict);
      Assert.NotNull(fromNoContent);
      Assert.NotNull(fromNotFound);
      Assert.NotNull(fromOk);
      Assert.NotNull(fromUnauthorized);
      Assert.NotNull(fromUnprocessableEntity);
      Assert.NotNull(unsupportedMediaType);
    }
  }
}
