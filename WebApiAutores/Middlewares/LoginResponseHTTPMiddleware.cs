using Microsoft.Extensions.Logging;

namespace WebApiAutores.Middlewares
{
    public static class LogingResponseHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogingResponeHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoginResponseHTTPMiddleware>();
        }
    }

    public class LoginResponseHTTPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoginResponseHTTPMiddleware> _logger;

        public LoginResponseHTTPMiddleware(RequestDelegate next, ILogger<LoginResponseHTTPMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        //Invoke o InvokeAsync

        public async Task InvokeAsync(HttpContext context)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = context.Response.Body;
                context.Response.Body = ms;
                await _next(context);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();

                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(cuerpoOriginalRespuesta);

                context.Response.Body = cuerpoOriginalRespuesta;
                _logger.LogInformation(respuesta);
            }
        }
    }
}
