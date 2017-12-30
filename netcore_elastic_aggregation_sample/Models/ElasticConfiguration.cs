using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Models
{
    public class ElasticConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string IndexName { get; set; }
    }
}
