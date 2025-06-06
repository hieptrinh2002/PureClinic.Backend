﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;

namespace PureLifeClinic.API.Extensions
{
    public static class HttpContextExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        public static async ValueTask WriteAccessDeniedResponse(
            this HttpContext context,
            string? title = null,
            int? statusCode = null,
            CancellationToken cancellationToken = default)
        {
            var problem = new ProblemDetails
            {
                Instance = context.Request.Path,
                Title = title ?? "Access denied",
                Status = statusCode ?? (int)HttpStatusCode.Forbidden,    
            };
            context.Response.StatusCode = problem.Status.Value;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonSerializerOptions),
                cancellationToken);
        }
    }
}
