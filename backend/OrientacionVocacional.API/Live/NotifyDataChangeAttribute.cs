using Microsoft.AspNetCore.Mvc.Filters;

namespace OrientacionVocacional.API.Live;

/// <summary>
/// Avisa a las pantallas abiertas cuando una peticion modifico datos con exito. Las consultas (GET) y las
/// peticiones rechazadas no avisan, para no provocar recargas sin motivo.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class NotifyDataChangeAttribute(DataArea area) : ResultFilterAttribute
{
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        var http = context.HttpContext;
        if (HttpMethods.IsGet(http.Request.Method) || context.Exception is not null || http.Response.StatusCode is < 200 or >= 300) return;
        http.RequestServices.GetRequiredService<DataVersion>().Bump(area);
    }
}
