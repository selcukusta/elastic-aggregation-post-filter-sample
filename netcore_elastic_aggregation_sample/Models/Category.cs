using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_elastic_aggregation_sample.Models
{
    public enum Category
    {
        [Description("Fantastik Kitaplar")]
        Fantastik = 1,
        [Description("Korku - Gerilim Kitapları")]
        KorkuGerilim,
        [Description("Macera Kitapları")]
        Macera
    }
}
