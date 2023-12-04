﻿using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace PulseStore.PL.Extensions;

public static class FluentValidationExtensions
{
    public static void UseFluentValidationExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(x =>
        {
            x.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature?.Error;

                if (exception is not ValidationException validationException)
                {
                    // Re-throw if it's not a ValidationException
                    throw exception!;
                }

                var errors = validationException.Errors.Select(err => new
                {
                    err.PropertyName,
                    err.ErrorMessage
                });

                var errorText = JsonSerializer.Serialize(errors);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(errorText, Encoding.UTF8);
            });
        });
    }
}