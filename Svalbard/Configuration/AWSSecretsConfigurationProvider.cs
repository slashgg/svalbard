using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Svalbard.Configuration
{
  public class AWSSecretsConfigurationProvider : ConfigurationProvider
  {
    public AWSSecretsConfigurationProvider(Action<AWSSecretsConfiguration> configAction)
    {
      ConfigAction = configAction;
    }

    public Action<AWSSecretsConfiguration> ConfigAction { get; }

    public override void Load()
    {
      var config = new AWSSecretsConfiguration();

      ConfigAction(config);

      MemoryStream memoryStream = new MemoryStream();

      // Initialize an empty secret store
      Data = new Dictionary<string, string>();

      IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(config.Region));
      foreach (var secretId in config.Secrets)
      {
        var request = new GetSecretValueRequest {
          SecretId = secretId
        };

        var response = Task.Run(() => client.GetSecretValueAsync(request)).Result;
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.SecretString);

        foreach (var kvp in data)
        {
          Data.Add($"{secretId}:${kvp.Key}", kvp.Value);
        }
      }
    }
  }
}
