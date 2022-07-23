using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class MLViewModel
    {
        public MycotoxinML mycotoxinML { get; set; }
        public string uom { get; set; }
    }
}