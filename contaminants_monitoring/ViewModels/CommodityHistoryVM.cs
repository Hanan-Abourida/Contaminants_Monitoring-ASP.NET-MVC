using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class CommodityHistoryVM
    {
        public List<CommodityHistoryItem> commodityHistoryItems { get; set; }

    public class CommodityHistoryItem
    {
        public string commodityName { get; set; }
        public int year { get; set; }
        public int nbOfSample { get; set; }
    }

    }
}