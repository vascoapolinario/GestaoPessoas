using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace GestaoPessoas.Converters
{
    public class TimeZoneInfoSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(TimeZoneInfo))
            {
                schema.Type = "string";
                schema.Format = "timezone";
                schema.Properties?.Clear();
                schema.Description = "Time zone identifier (e.g. 'UTC', 'GMT Standard Time', 'Pacific Standard Time')";
                schema.Example = new OpenApiString("UTC");
            }
        }
    }
}