using Microsoft.Extensions.Options;
using netcore_elastic_aggregation_sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using netcore_elastic_aggregation_sample.Elastic.Types;

namespace netcore_elastic_aggregation_sample.Elastic
{
    public class Elastic : IElastic
    {
        private readonly ElasticConfiguration _configuration;

        public Elastic(ElasticConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticClient Client
        {
            get
            {
                var uriBuilder = new UriBuilder
                {
                    Host = _configuration.Host,
                    Port = _configuration.Port
                };

                var esNodesettings = new ConnectionSettings(uriBuilder.Uri);
                esNodesettings.DefaultIndex(_configuration.IndexName);
                var esClient = new ElasticClient(esNodesettings);
                return esClient;
            }
        }

        public bool AddBulkData<T>(IList<T> contents) where T : BaseElasticType, new()
        {
            if (contents == null || !contents.Any())
            {
                return false;
            }

            List<IBulkOperation> operations = new List<IBulkOperation>();
            operations.AddRange(contents.Select(c => new BulkUpdateOperation<T, T>(c, c, true)));

            var request = new BulkRequest()
            {
                Refresh = Elasticsearch.Net.Refresh.True,
                Operations = operations
            };

            var response = Client.Bulk(request);
            return !response.Errors;
        }

        public IndexStatus CreateIndex<T>() where T : BaseElasticType, new()
        {
            if (Client.IndexExists(_configuration.IndexName).Exists)
            {
                return IndexStatus.AlreadyExists;
            }

            var defaultAnalyzer = new CustomAnalyzer
            {
                Filter = new List<string> { "lowercase", "asciifolding", "word_delimiter" },
                CharFilter = new List<string> { "html_strip" },
                Tokenizer = "standard"
            };

            var createIndexResponse = Client.CreateIndex(_configuration.IndexName, config => config
                .Settings(s => s
                    .NumberOfShards(1)
                    .NumberOfReplicas(0)
                    .Analysis(a => a
                        .Analyzers(b => b
                        .UserDefined("default", defaultAnalyzer)
                    )
                ))
                .Mappings(m => m
                    .Map<T>(d => d
                        .AutoMap()
                    )
                ));

            return createIndexResponse.Acknowledged ? IndexStatus.Created : IndexStatus.Failed;
        }
    }
}