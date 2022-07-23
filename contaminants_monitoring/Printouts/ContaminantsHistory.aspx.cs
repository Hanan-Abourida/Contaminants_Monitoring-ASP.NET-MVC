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
    public partial class ContaminantsHistory : System.Web.UI.Page
    {
        private static FoodSafetyDBEntities db = new FoodSafetyDBEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            //get the id from query string
           // int idParam = int.Parse(Request.QueryString["id"]);

          //  DisableUnwantedExportFormat(ReportViewer1, "Excel");
          //  DisableUnwantedExportFormat(ReportViewer1, "WORD");

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("ContaminantsHistory.rdlc");
            var queryResult = DataAccess.GetContaminantHistory();
            ReportDataSource dataSource = new ReportDataSource("ContaminantsHistoryDS", queryResult);
            //ReportViewer1.LocalReport.DisplayName = "Complaint" + "_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + queryResult.First().Code;
            //string arabicDate = queryResult.First().DateOfSubmission.Value.ToString("yyyy, dddd, dd MMMM", new CultureInfo("ar-LB"));
            //ReportParameter parameter = new ReportParameter("arabicDate", arabicDate);
            //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { parameter });

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dataSource);
        }
    }
}