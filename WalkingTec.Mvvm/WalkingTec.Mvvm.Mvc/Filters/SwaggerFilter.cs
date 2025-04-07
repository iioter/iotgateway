using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Filters
{
    public class SwaggerFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type == typeof(List<ComboSelectListItem>))
            {
                schema = null;
            }
            //if (type.IsEnum)
            //{
            //    schema.Extensions.Add(
            //        "x-ms-enum",
            //        new OpenApiObject
            //        {
            //            ["name"] = new OpenApiString(type.Name),
            //            ["modelAsString"] = new OpenApiBoolean(true)
            //        }
            //    );
            //};
        }
    }
}