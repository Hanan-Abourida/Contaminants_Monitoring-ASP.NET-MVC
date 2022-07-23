using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Contaminants_Monitoring.Printouts
{
    public partial class FindingsSummaryRpt_NoGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["ModelParam"] != null)
            {
            SearchViewModel model = (SearchViewModel)Session["ModelParam"];

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("FindingsSummary_NoGroup.rdlc");
            ReportDataSource dataSource = new ReportDataSource("FindingsSummaryDs", model.findingsStats);
            
            ReportViewer1.LocalReport.DisplayName = "Summary_Of_Findings" + "_" + DateTime.Now.ToString("yyyy-MM-dd");

               // paramaters to show the selected data filters
                ReportParameter SelectedCommodities;
                if (model.selectedCommodities != null && model.selectedCommodities.Contains(0))
                    SelectedCommodities = new ReportParameter("SelectedCommodities", "Apple - Pistachio");
                else if (model.selectedCommodities != null)
                    SelectedCommodities = new ReportParameter("SelectedCommodities", getSelectedCommoditiesString(model.selectedCommodities.ToList()));
                else SelectedCommodities = new ReportParameter("SelectedCommodities", "");

                ReportParameter SelectedPesticideResidues;
                if (model.selectedPesticideResidues != null && model.selectedPesticideResidues.Contains(0))
                    SelectedPesticideResidues = new ReportParameter("SelectedPestRes", "All Pesticides Residues");
                else if (model.selectedPesticideResidues != null)
                    SelectedPesticideResidues = new ReportParameter("SelectedPestRes", getSelectedPesticideResiduesString(model.selectedPesticideResidues.ToList()));
                else SelectedPesticideResidues = new ReportParameter("SelectedPestRes", "");

                ReportParameter SelectedMycotoxins;
                if (model.selectedMycotoxins != null && model.selectedMycotoxins.Contains(0))
                    SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", "All Mycotoxins");
                else if(model.selectedMycotoxins != null) 
                    SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", getSelectedMycotoxinsString(model.selectedMycotoxins.ToList()));
                else SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", "");

                ReportParameter SelectedYears;
                if (model.selectedYears != null && model.selectedYears.Contains(0))
                    SelectedYears = new ReportParameter("SelectedYears", "All Years");
                else if (model.selectedYears != null)
                    SelectedYears = new ReportParameter("SelectedYears", getSelectedYearsString(model.selectedYears.ToList()));
                else SelectedYears = new ReportParameter("SelectedYears", "");

                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { SelectedCommodities, SelectedMycotoxins, SelectedPesticideResidues, SelectedYears });
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dataSource);
            }
        }

        public static string getSelectedCommoditiesString(List<int?> commodities)
        {
            string paramStr = "";
            foreach (int i in commodities)
            {
                Commodity commodity = DataAccess.getCommodityFromId(i);
                if (commodity != null)
                {
                    if(string.IsNullOrEmpty(paramStr))
                        paramStr = paramStr + "( " + commodity.CommodityCode + " ) " + commodity.CommodityName;
                    else paramStr = paramStr + " - ( " + commodity.CommodityCode + " ) " + commodity.CommodityName;
                }
                    
            }
            return paramStr;
        }

        public static string getSelectedPesticideResiduesString(List<int?> pesticideResidues)
        {
            string paramStr = "";
            foreach (int i in pesticideResidues)
            {
                PesticideResidue pesticideResidue = DataAccess.GetPesticideResidueFromId(i);
                if (pesticideResidue != null)
                {
                    if (string.IsNullOrEmpty(paramStr))
                        paramStr = paramStr + pesticideResidue.PestResName;
                    else paramStr = paramStr + " - " + pesticideResidue.PestResName;
                }
            }
            return paramStr;
        }

        public static string getSelectedMycotoxinsString(List<int?> myCotoxins)
        {
            string paramStr = "";
            foreach (int i in myCotoxins)
            {
                Mycotoxin mycotoxin = DataAccess.GetMycotoxinFromId(i);
                if (mycotoxin != null)
                {
                    if (string.IsNullOrEmpty(paramStr))
                        paramStr = paramStr + mycotoxin.MycotoxinName;
                    else paramStr = paramStr + " - " + mycotoxin.MycotoxinName;
                }

            }
            return paramStr;
        }

        public static string getSelectedYearsString(List<int?> years)
        {
            string paramStr = "";
            foreach (int i in years)
            {
                if (string.IsNullOrEmpty(paramStr))
                    paramStr = paramStr + i.ToString();
                else paramStr = paramStr + " - " + i.ToString();
            }
            return paramStr;
        }
    }
}