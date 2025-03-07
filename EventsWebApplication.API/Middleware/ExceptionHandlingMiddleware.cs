﻿
using EventsWebApplication.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventsWebApplication.API.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                var statusCode = ex switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    AlreadyExistsException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                var details = new ProblemDetails
                {
                    Title = "Error",
                    Type = ex.GetType().Name,
                    Status = statusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(details);

                await context.Response.WriteAsync(json);

            }
        }
    }
}
