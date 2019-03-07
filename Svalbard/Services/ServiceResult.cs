using System.Collections;
using System.Collections.Generic;

namespace Svalbard.Services
{
  public class ServiceResult<T> : ServiceResult
  {
    public T Data { get; set; }

    public ServiceResult() { }

    public ServiceResult(bool success) : base(success) { }
    public ServiceResult(ServiceError error) : base(error) { }

    public ServiceResult(T data)
    {
      this.Data = data;
    }

    public void Succeed(T data)
    {
      this.Data = data;
      base.Succeed();
    }
  }

  public class ServiceResult
  {
    public ServiceError Error { get; set; }
    public IList<FieldError> FieldErrors { get; set; } = new List<FieldError>();
    public bool Success { get; set; } = false;

    public ServiceResult() { }

    public ServiceResult(bool success)
    {
      this.Success = success;
    }

    public ServiceResult(ServiceError error)
    {
      this.Error = error;
      this.Success = false;
    }

    public void Succeed()
    {
      this.Success = true;
      this.Error = null;
    }
  }
}
