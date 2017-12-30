using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using netcore_elastic_aggregation_sample.Models;
using netcore_elastic_aggregation_sample.Elastic;
using netcore_elastic_aggregation_sample.Elastic.Types;

namespace netcore_elastic_aggregation_sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var elasticConfig = new ElasticConfiguration
            {
                Host = Configuration.GetSection("ElasticConfiguration").GetValue<string>("Host"),
                IndexName = Configuration.GetSection("ElasticConfiguration").GetValue<string>("IndexName"),
                Port = Configuration.GetSection("ElasticConfiguration").GetValue<int>("Port")
            };
            services.AddScoped<IElastic>(service => new Elastic.Elastic(elasticConfig));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IElastic elastic)
        {

            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();

            if (elastic.CreateIndex<ProductType>() == IndexStatus.Created)
            {
                elastic.AddBulkData<ProductType>(new List<ProductType> {
                    new ProductType {ProductName = "Aeden (Bir Dünya Hikayesi) - Azra Kohen", CategoryId=1, Price = 21.50},
                    new ProductType {ProductName = "Taht Oyunları (9 Kitap) - George R. R. Martin", CategoryId=1, Price = 199.00},
                    new ProductType {ProductName = "Yüzüklerin Efendisi (3 Kitap Tek Cilt) - J. R. R. Tolkien", CategoryId=1, Price = 86.86},
                    new ProductType {ProductName = "Kırmızı Piyano - Josh Malerman", CategoryId=2, Price = 12.50},
                    new ProductType {ProductName = "Başlangıç - Dan Brown", CategoryId=3, Price = 26.60},
                    new ProductType {ProductName = "Kongo'ya Aıt - Jean Christophe Grange", CategoryId=3, Price = 22.40},
                    new ProductType {ProductName = "Uykusuzlar - Gülşah Elikbank", CategoryId=3, Price = 14.00},
                    new ProductType {ProductName = "Psikopat - Sir Arthur Conan Doyle", CategoryId=3, Price = 17.40}
                });
            }


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
