using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class CommoditySeparatorDictionary
    {
        public string commodity { get; set; }
        public Dictionary<string, NumberPercentageVM> samplesNbForChemical{ get; set; }
        public string separator { get; set; }

    }
}