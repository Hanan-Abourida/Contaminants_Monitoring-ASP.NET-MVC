using Contaminants_Monitoring.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Contaminants_Monitoring.Printouts
{
    public partial class CommoditiesHistory : System.Web.UI.Page
    {
        private static FoodSafetyDBEntities db = new FoodSafetyDBEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("CommoditiesHistory.rdlc");
            var queryResult = DataAccess.GetCommodityHistory();
            ReportDataSource dataSource = new ReportDataSource("CommoditiesHistoryDS", queryResult);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dataSource);
        }
    }
}