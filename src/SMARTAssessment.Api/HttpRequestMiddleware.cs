using System.Net;

namespace SMARTAssessment.Api;

public class HttpRequestMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpRequestException e)
        {
            context.Response.StatusCode = (int)e.StatusCode!;
            await context.Response.WriteAsJsonAsync(new{ error = e.Message });
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new{ error = e.Message });
        }
    }
}