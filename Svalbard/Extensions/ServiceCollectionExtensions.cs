using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Svalbard
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddSvalbard(this IServiceCollection collection)
    {
      collection.AddTransient<GlobalExceptionFilter>();
      collection.Configure<ApiBehaviorOptions>(options =>
      {
        options.SuppressModelStateInvalidFilter = true;
      });

      collection.Configure<MvcOptions>(options =>
      {
        // Add global operation exception filter
        options.Filters.Add<GlobalExceptionFilter>();
      });

      return collection;
    }
  }
}
