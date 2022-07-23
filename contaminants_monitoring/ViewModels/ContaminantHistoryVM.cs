using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class ContaminantHistoryVM
    {
        public List<ContaminantHistoryItem> contaminantHistoryItems { get; set; }
    }

    public class ContaminantHistoryItem
    {
        public string contaminant { get; set; }
        public int year { get; set; }
        public int nbOfProducts { get; set; }
    }
}