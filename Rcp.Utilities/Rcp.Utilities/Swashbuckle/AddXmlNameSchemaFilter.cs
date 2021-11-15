using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rcp.Utilities
{
    public class AddXmlNameSchemaFilter : ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiSchema       schema,
                          SchemaFilterContext context)
        {
            if (schema.Properties == null) return;


            foreach (var entry in schema.Properties)
            {
                var properties = context.Type.GetProperties();


                var name = properties.FirstOrDefault(x => x.Name.ToLower() == entry.Key.ToLower())
                                    ?.Name;

                if (name == null)
                {
                    foreach (var p in properties)
                    {
                        var attribute = (XmlElementAttribute)p.GetCustomAttribute(typeof(XmlElementAttribute),
                            false);
                        if (attribute == null) continue;
                        if (!string.Equals(attribute.ElementName,
                                           entry.Key,
                                           StringComparison.CurrentCultureIgnoreCase)) continue;
                        name = attribute.ElementName;
                        break;
                    }
                }

                entry.Value.Xml = new OpenApiXml
                                  {
                                      Name = name
                                  };

                if (entry.Value.Reference != null)
                {
                    entry.Value.AllOf.Add(context.SchemaRepository.Schemas[entry.Value.Reference.Id]);

                    entry.Value.Reference = null;
                }
            }
        }
    }
}
