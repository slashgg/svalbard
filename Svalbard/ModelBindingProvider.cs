using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Svalbard
{
  public class ModelBindingProvider : IModelBinderProvider
  {
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
      return new ModelBinder();
    }

    public class ModelBinder : IModelBinder
    {
      public Task BindModelAsync(ModelBindingContext bindingContext)
      {
        if (bindingContext == null)
        {
          throw new ArgumentNullException(nameof(bindingContext));
        }
        
        throw new NotImplementedException();
      }
    }
  }
}
