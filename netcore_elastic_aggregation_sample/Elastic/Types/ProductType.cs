using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Elastic.Types
{
    [ElasticsearchType(Name = "product")]
    public class ProductType : BaseElasticType
    {
        /// <summary>
        ///     Produt Name
        /// </summary>
        [Text(Name = "product_name")]
        public string ProductName { get; set; }

        /// <summary>
        ///     Category ID
        /// </summary>
        [Number(NumberType.Integer, Name = "category_id")]
        public int CategoryId { get; set; }

        /// <summary>
        ///     Price
        /// </summary>
        [Number(NumberType.Double, Name = "price")]
        public double Price { get; set; }
    }
}
