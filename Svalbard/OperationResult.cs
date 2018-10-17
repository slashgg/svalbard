using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svalbard
{
  public class OperationResult<T> : IActionResult
  {
    private readonly T _data;
    private readonly Exception _exception;
    private readonly int _statusCode;

    public OperationResult()
    {
      _data = default(T);
    }

    public OperationResult(T data)
    {
      _data = data;
    }

    public OperationResult(Exception e)
    {
      _exception = e;
    }

    private OperationResult(int statusCode)
    {
      _statusCode = statusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
      var response = context.HttpContext.Response;
      response.ContentType = "application/json; charset=utf-8";

      if (_exception != null)
      {
        response.StatusCode = 500;

        var error = new Error();
        error.Message = _exception.Message;
        error.Code = "ServerError";

        await WritePayload(error, context.HttpContext);

        return;
      }

      if (!context.ModelState.IsValid && _statusCode == 400)
      {
        response.StatusCode = 400;

        var error = new Error();
        error.Message = "Something was wrong with your input.";
        error.Code = "InvalidModel";

        foreach (var item in context.ModelState.Where(ms => ms.Value.Errors.Any()))
        {
          error.Fields.Add(new FieldError
          {
            AttemptedValue = item.Value.AttemptedValue,
            Key = item.Key,
            Messages = item.Value.Errors.Select(e => e.ErrorMessage),
          });

          await WritePayload(error, context.HttpContext);

          return;
        }
      }

      if (_statusCode > 0)
      {
        response.StatusCode = _statusCode;
        return;
      }

      response.StatusCode = _data != null ? 200 : 204;

      if (response.StatusCode == 200)
      {
        await WritePayload(_data, context.HttpContext);
      }
    }
    
    public static implicit operator OperationResult<T>(T other) => new OperationResult<T>(other);
    public static implicit operator OperationResult<T>(BadRequestResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(ConflictResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(NoContentResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(NotFoundResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(OkResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(UnauthorizedResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(UnprocessableEntityResult other) => new OperationResult<T>(other.StatusCode);
    public static implicit operator OperationResult<T>(UnsupportedMediaTypeResult other) => new OperationResult<T>(other.StatusCode);

    private async Task WritePayload(object data, HttpContext context)
    {
      var payload = JsonConvert.SerializeObject(data);
      await context.Response.WriteAsync(payload, Encoding.UTF8, context.RequestAborted);
    }
  }
}
