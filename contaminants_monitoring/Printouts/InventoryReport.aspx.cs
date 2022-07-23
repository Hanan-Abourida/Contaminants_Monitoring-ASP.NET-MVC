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
    public partial class InventoryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("LabInventory.rdlc");
            List<LabSample> result = (List<LabSample>)Session["LabInventory"];
         

            ReportDataSource dataSource = new ReportDataSource("LabInventoryDS", result);

            ReportViewer1.LocalReport.DisplayName = "LabInventory" + "_" + DateTime.Now.ToString("yyyy_MM_dd");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dataSource);
        }
    }
}