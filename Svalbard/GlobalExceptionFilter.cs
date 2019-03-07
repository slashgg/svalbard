using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Svalbard
{
  public class GlobalExceptionFilter : IExceptionFilter
  {
    private readonly ILogger<GlobalExceptionFilter> logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
      this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
      if (!context.ExceptionHandled)
      {
        logger.LogCritical(context.Exception, "Global exception caught");
        context.Result = new OperationResult<Error>(context.Exception);
      }
    }
  }
}
