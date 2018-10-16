using Microsoft.AspNetCore.Mvc.Filters;

namespace Svalbard
{
  public class GlobalExceptionFilter : IExceptionFilter
  {
    public void OnException(ExceptionContext context)
    {
      if (!context.ExceptionHandled)
      {
        context.Result = new OperationResult<Error>(context.Exception);
      }
    }
  }
}
