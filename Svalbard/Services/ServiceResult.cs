namespace Svalbard.Services
{
  public class ServiceResult<T> : ServiceResult
  {
    public T Data { get; set; }

    public ServiceResult() { }

    public ServiceResult(bool success) : base(success) { }
    public ServiceResult(string errorKey) : base(errorKey) { }

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
    public string ErrorKey { get; set; }
    public bool Success { get; set; } = false;

    public ServiceResult() { }

    public ServiceResult(bool success)
    {
      this.Success = success;
    }

    public ServiceResult(string errorKey)
    {
      this.ErrorKey = errorKey;
      this.Success = false;
    }

    public void Succeed()
    {
      this.Success = true;
      this.ErrorKey = null;
    }
  }
}
