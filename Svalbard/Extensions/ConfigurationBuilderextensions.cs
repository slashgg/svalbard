using Microsoft.Extensions.Configuration;
using Svalbard.Configuration;
using System;

namespace Svalbard.Extensions
{
  public static class ConfigurationBuilderExtensions
  {
    public static IConfigurationBuilder AddAWSSecrets(this IConfigurationBuilder builder, Action<AWSSecretsConfiguration> configAction)
    {
      return builder.Add(new AWSSecretsConfigurationSource(configAction));
    }
  }
}
