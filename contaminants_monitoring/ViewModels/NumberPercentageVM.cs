using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contaminants_Monitoring.ViewModels
{
    public class NumberPercentageVM
    {
        public int number { get; set; }
        public double percentage { get; set; }

        public NumberPercentageVM(int number, double percentage)
        {
            this.number = number;
            this.percentage = percentage;
        }
        public NumberPercentageVM()
        {

        }
    }
}