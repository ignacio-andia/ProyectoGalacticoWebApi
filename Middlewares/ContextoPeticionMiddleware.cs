using Serilog.Context;

namespace ProyectoGalacticoWebApi.Middlewares;

public class ContextoPeticionMiddleware
{
    private readonly RequestDelegate _next;

    public ContextoPeticionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
        using (LogContext.PushProperty("Metodo", context.Request.Method))
        using (LogContext.PushProperty("Ruta", context.Request.Path))
        using (LogContext.PushProperty("IP", context.Connection.RemoteIpAddress?.ToString()))
        {
            await _next(context);
        }
    }
}
