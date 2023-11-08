using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filters
{
    public class MiFiltro : IActionFilter
    {
        private readonly ILogger<MiFiltro> _logger;

        public MiFiltro(ILogger<MiFiltro> logger)
        {
            this._logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Antes de ejecutar el metodo o accion");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Despues de ejecutar el metodo o accion");
        }
    }
}
