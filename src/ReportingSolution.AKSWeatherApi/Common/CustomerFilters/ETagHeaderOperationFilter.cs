using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReportingSolution.AKSWeatherApi.Common.CustomerFilters
{
    public class ETagHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Apply only to PUT methods
            if (context.ApiDescription.HttpMethod?.ToLower() != "put")
            {
                return;
            }

            operation.Parameters ??= [];
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "If-Match",
                In = ParameterLocation.Header,
                Required = true,
                Description = "ETag value from previous GET to ensure optimistic concurrency.",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("\"0000abcd-0000-0000-0000-638f1f7c0000\"")
                }
            });
        }
    }
}
