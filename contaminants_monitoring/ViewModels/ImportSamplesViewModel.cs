using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class ImportSamplesViewModel
    {
        public int fkCommodityId { get; set; }
        public int fkCommodityStateId { get; set; }
        public IEnumerable<Commodity> commodities { get; set; }
    }
}