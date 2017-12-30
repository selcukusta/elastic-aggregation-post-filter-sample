using Nest;
using netcore_elastic_aggregation_sample.Elastic.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Elastic
{
    public interface IElastic
    {
        ElasticClient Client { get; }
        IndexStatus CreateIndex<T>() where T : BaseElasticType, new();
        bool AddBulkData<T>(IList<T> contents) where T : BaseElasticType, new();
    }
}
