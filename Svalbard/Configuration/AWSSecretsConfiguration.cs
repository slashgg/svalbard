namespace Svalbard.Configuration
{
  public class AWSSecretsConfiguration
  {
    /// <summary>
    /// An array of names for the secrets we want to access.
    /// </summary>
    public string[] Secrets { get; set; }
    public string Region { get; set; }
  }
}
