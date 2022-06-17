using System.Text;

namespace $ext_safeprojectname$.Api.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = await FormatRequest(context.Request);

        var originalBodyStream = context.Response.Body;

        try
        {
            await using var responseBody = new MemoryStream();

            context.Response.Body = responseBody;

            await _next(context);

            if (context.Response.StatusCode > 399)
            {
                // Format the response from the server
                var response = await FormatResponse(context.Response);

                _logger.LogWarning($"{request}\t{response}");
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            _logger.LogError($"{request}\n\n{e.Message}\n\n{e.StackTrace}");

            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        string bodyAsText;

        // Allows using several time the stream in ASP.Net Core
        request.EnableBuffering();

        using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
        {
            bodyAsText = await reader.ReadToEndAsync();
        }

        // Rewind, so the core is not lost when it looks the body for the request
        request.Body.Position = 0;

        return $"{request.Scheme} {request.Method} {request.Host}{request.Path}{request.QueryString} {bodyAsText}";
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        // We need to read the response stream from the beginning...
        response.Body.Seek(0, SeekOrigin.Begin);

        // ...and copy it into a string
        string text = await new StreamReader(response.Body).ReadToEndAsync();

        // We need to reset the reader for the response so that the client can read it.
        response.Body.Seek(0, SeekOrigin.Begin);

        // Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
        return $"{response.StatusCode}: {text}";
    }
}