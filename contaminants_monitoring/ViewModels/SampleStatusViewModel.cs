using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class SampleStatusViewModel
    {
        public int labSampleId { get; set; }
        public string identityCode { get; set; }
        public string status { get; set; }
        public string commodity { get; set; }
        public string contaminantType { get; set; }
        public string designerNotes { get; set; }
        public string collector { get; set; }
        public string laboratoty { get; set; }
        public string generatedDate { get; set; }
        public string governorate { get; set; }
        public string caza { get; set; }
        public string collectionNotes { get; set; }
        public string dispatchDate { get; set; }

    }
}