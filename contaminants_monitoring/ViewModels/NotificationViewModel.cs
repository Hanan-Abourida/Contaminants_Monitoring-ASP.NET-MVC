using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class NotificationViewModel
    {
        public List<ApplicationUser> selectedCollectors { get; set; }
        public List<ApplicationUser> allCollectors { get; set; }

    }
}