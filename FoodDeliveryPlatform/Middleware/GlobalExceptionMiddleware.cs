using System.Net;
using System.Text.Json;
using FDP.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FDP.Middleware;
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger
    )
    {
        _next=next;
        _logger=logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await  HandleExceptionAsync(context,ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception
    )
    {
        context.Response.ContentType="application/json";

        var statusCode=exception switch
        {
            NotFoundException=>StatusCodes.Status404NotFound,
            BadRequestException=>StatusCodes.Status400BadRequest,
            UnauthorizedAccessException=>StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            StatusCode=statusCode,
            message=exception switch
            {
                NotFoundException=>exception.Message,
                BadRequestException=>exception.Message,
                UnauthorizedAccessException=>exception.Message,

                _=>"An unexpected error occured"
            }
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(context)
        );
    }
}