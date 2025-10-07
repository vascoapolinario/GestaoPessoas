using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace GestaoPessoas.Documentacao.OpenApi
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
                schema.Description = "Time zone identifier (e.g. 'UTC', 'America/Los Angeles', 'Europe/Lisbon')";
                schema.Example = new OpenApiString("UTC");
            }
        }
    }
}