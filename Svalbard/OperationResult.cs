using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svalbard
{
  public class OperationResult<T> : OperationResult
  {
    protected readonly T _data;
    public OperationResult()
    {
      _data = default(T);
    }

    public OperationResult(string errorKey) : base(errorKey)
    {
    }

    public OperationResult(T data, string errorKey = null) : base(errorKey)
    {
      _data = data;
    }

    public OperationResult(Exception e, string errorKey = null) : base(e, errorKey)
    {
    }

    public OperationResult(int statusCode, string errorKey = null) : base(statusCode, errorKey)
    {
    }

    public override async Task ExecuteResultAsync(ActionContext context)
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
        if (_errorKey != null)
        {
          await WritePayload(new { error = _errorKey }, context.HttpContext);
        }
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

  }
  public class OperationResult : IActionResult
  {
    protected readonly Exception _exception;
    protected readonly int _statusCode;
    protected readonly string _errorKey;

    public OperationResult()
    {
    }

    public OperationResult(string errorKey)
    {
      _errorKey = errorKey;
    }

    public OperationResult(Exception e, string errorKey = null)
    {
      _exception = e;
      _errorKey = errorKey;
    }

    public OperationResult(int statusCode, string errorKey = null)
    {
      _statusCode = statusCode;
      _errorKey = errorKey;
    }

    public virtual async Task ExecuteResultAsync(ActionContext context)
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
        if (_errorKey != null)
        {
          await WritePayload(new { error = _errorKey }, context.HttpContext);
        }
        return;
      }

      response.StatusCode = 204;
    }
    
    public static implicit operator OperationResult(BadRequestResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(ConflictResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(NoContentResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(NotFoundResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(OkResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(UnauthorizedResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(UnprocessableEntityResult other) => new OperationResult(other.StatusCode);
    public static implicit operator OperationResult(UnsupportedMediaTypeResult other) => new OperationResult(other.StatusCode);

    protected async Task WritePayload(object data, HttpContext context)
    {
      var payload = JsonConvert.SerializeObject(data);
      await context.Response.WriteAsync(payload, Encoding.UTF8, context.RequestAborted);
    }
  }
}
