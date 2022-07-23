using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class FindingsStat
    {
        public string molecule { get; set; }
        public string commodity { get; set; }

        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double mean { get; set; }

        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double standardDeviation { get; set; }
        public double minimum { get; set; }
        public double maximum { get; set; }

        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double range { get; set; }

        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double median { get; set; }

        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double quantile1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double quantile2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double quantile3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double quantile4 { get; set; }


        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double percentile5 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double percentile25 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double percentile75 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double percentile90 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double percentile95 { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}", ApplyFormatInEditMode = true)]
        public double coefficientOfVariation { get; set; }

    }
}