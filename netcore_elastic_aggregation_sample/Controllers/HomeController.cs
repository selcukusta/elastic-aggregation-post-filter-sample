using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using netcore_elastic_aggregation_sample.Elastic;
using netcore_elastic_aggregation_sample.Elastic.Types;
using System.Net;
using netcore_elastic_aggregation_sample.Models;
using netcore_elastic_aggregation_sample.Extensions;
using Nest;

namespace netcore_elastic_aggregation_sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IElastic _elastic;

        public HomeController(IElastic elastic)
        {
            _elastic = elastic;
        }

        public IActionResult Index(int? id)
        {
            var searchDescriptor = new SearchDescriptor<ProductType>();
            searchDescriptor.From(0);
            searchDescriptor.Size(10);
            searchDescriptor.Aggregations
            (
                agg => agg.Terms
                (
                    "categoriesWithProductCount",
                    term => term.Field(f => f.CategoryId)
                )
            );

            if (id.HasValue)
            {
                #region ".PostFilter yerine .Query içerisine eklenen filtre, aggregation yapısında değişikliğe neden olacaktır!
                //searchDescriptor.Query
                //(
                //    q => q.Term
                //    (
                //        term => term.Field(f => f.CategoryId).Value(id)
                //    )
                //);
                #endregion

                searchDescriptor.PostFilter
                (
                    filter => filter.Term
                    (
                        term => term.Field(f => f.CategoryId).Value(id)
                    )
                );
            }
            var searchResults = _elastic.Client.Search<ProductType>(c => searchDescriptor);

            if (searchResults?.Documents == null || !searchResults.Documents.Any())
            {
                return StatusCode((int)HttpStatusCode.NoContent);
            }

            ViewBag.Documents = searchResults.Documents;

            if (searchResults?.Aggs?.Terms("categoriesWithProductCount")?.Buckets != null)
            {
                var buckets = searchResults.Aggs.Terms("categoriesWithProductCount").Buckets;
                var menuItems = new List<MenuItem>();
                foreach (var item in buckets)
                {
                    var categoryName = Enum.Parse<Category>(item.Key).Description();
                    var documentCount = item.DocCount.GetValueOrDefault(0);
                    menuItems.Add(new MenuItem { RouteValue = item.Key, ItemCount = documentCount, Title = categoryName });
                }
                ViewBag.MenuItems = menuItems;
            }

            return View();
        }
    }
}

