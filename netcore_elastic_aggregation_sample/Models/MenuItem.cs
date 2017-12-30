using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Models
{
    public class MenuItem
    {
        public string RouteValue { get; set; }
        public string Title { get; set; }
        public long ItemCount { get; set; }
    }
}
