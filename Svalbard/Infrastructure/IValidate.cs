namespace Svalbard.Infrastructure
{
  public interface IValidate
  {
    bool IsInvalid(object value);
    string GetErrorMessage();
  }
}