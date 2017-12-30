using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Elastic.Types
{
    public abstract class BaseElasticType
    {
        /// <summary>
        ///     Product Id
        /// </summary>
        [Keyword(Name = "id")]
        public string Id => Guid.NewGuid().ToString("N");
    }
}
