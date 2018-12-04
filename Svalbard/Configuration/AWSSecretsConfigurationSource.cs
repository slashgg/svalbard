using Microsoft.Extensions.Configuration;
using System;

namespace Svalbard.Configuration
{
  public class AWSSecretsConfigurationSource : IConfigurationSource
  {
    private readonly Action<AWSSecretsConfiguration> configAction;

    public AWSSecretsConfigurationSource(Action<AWSSecretsConfiguration> configAction)
    {
      this.configAction = configAction;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
      return new AWSSecretsConfigurationProvider(configAction);
    }
  }
}
