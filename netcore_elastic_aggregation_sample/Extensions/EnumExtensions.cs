using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Extensions
{
    public static class EnumExtensions
    {
        public static string Description(this Enum @enum)
        {
            var description = string.Empty;
            var fields = @enum.GetType().GetFields();
            foreach (var field in fields)
            {
                var descriptionAttribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descriptionAttribute != null && field.Name.Equals(@enum.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    description = descriptionAttribute.Description;
                    break;
                }
            }
            return description;
        }
    }
}
