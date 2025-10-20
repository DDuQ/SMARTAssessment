using Microsoft.AspNetCore.Http;

namespace SMARTAssessment.Infrastructure;

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
            await context.Response.WriteAsync(new{ error = e.Message });
        }
    }
}