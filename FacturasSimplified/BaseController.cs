using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facturas_simplified
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        protected async Task<ValidationResult> ValidateAsync<T>(T instance)
{
    var validator = HttpContext.RequestServices.GetService<IValidator<T>>();
    if (validator == null)
    {
        throw new ArgumentException($"No validator found for {typeof(T).Name}");
    }
    var validationContext = new ValidationContext<T>(instance);

    var result = await validator.ValidateAsync(validationContext);
    return result;
}
    }
}
