using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class CollectorSelectorViewModel
    {
        public int labSampleId { get; set; }
        public string selectedCollectorId { get; set; }
        public int selectedLabId { get; set; }
        public List<Laboratory> Laboratories { get; set; }
        public List<ApplicationUser> collectors { get; set; }

        public ApplicationUser selectedApplicationUser { get; set; }

        public DateTime? DueDate { get; set; }
    }
}