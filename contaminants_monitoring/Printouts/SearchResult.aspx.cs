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
    public partial class SearchResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("SamplesList.rdlc");
            List<LabSample> labSamples = (List<LabSample>)Session["LabSamples"];
            List<getLabSamplesBetweenDates_Result> result = new List<getLabSamplesBetweenDates_Result>();
            foreach (LabSample sample in labSamples)
            {
                getLabSamplesBetweenDates_Result newSampleResult = new getLabSamplesBetweenDates_Result();
                newSampleResult = MergeNewSampleFromSession(newSampleResult, sample);
                result.Add(newSampleResult);
            }
            //var queryResult = DataAccess.GetLabSamplesBetweenDates(DateTime.Parse("3/1/2020"), null);
            //var queryResult = result;
            ReportDataSource dataSource = new ReportDataSource("SamplesListDS", result);

            ReportViewer1.LocalReport.DisplayName = "LabSamples" + "_" + DateTime.Now.ToString("yyyy_MM_dd");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dataSource);
        }

        private static getLabSamplesBetweenDates_Result MergeNewSampleFromSession(getLabSamplesBetweenDates_Result sampleResult, LabSample labSample)
        {
            sampleResult.SampleNb = labSample.SampleNb;
            sampleResult.AnalysisDate = labSample.AnalysisDate;
            sampleResult.AnalysisPortion = labSample.AnalysisPortion;
            sampleResult.AnalysisType = labSample.AnalysisType;
            sampleResult.ConFinal = labSample.ConFinal;
            sampleResult.unit = labSample.fkConUOM == null ? "" : DataAccess.GetUOMTextFromId((int)labSample.fkConUOM);
            sampleResult.Uncertainty = labSample.Uncertainty;
            sampleResult.DeterminativeMethod = labSample.DeterminativeMethod;
            sampleResult.ExtractionMethod = labSample.ExtractionMethod;
            sampleResult.CommodityName = labSample.Commodity == null ? "" : labSample.Commodity.CommodityName;
            sampleResult.CommodityDescription = labSample.CommodityState == null ? "" : labSample.CommodityState.CommodityDescription;
            sampleResult.GovernorateName = labSample.Governorate.GovernorateName;
            sampleResult.LOD = labSample.LOD;
            sampleResult.LOQ = labSample.LOQ;
            
            sampleResult.MycotoxinName = labSample.Mycotoxin == null ? "" : labSample.Mycotoxin.MycotoxinName;
            sampleResult.PackQuantitySize = labSample.PackQuantitySize;
            sampleResult.PestResName = labSample.PesticideResidue == null ? "" : labSample.PesticideResidue.PestResName;
            sampleResult.PremiseName = labSample.PremiseName;
            sampleResult.SamplingDate = labSample.SamplingDate;
            sampleResult.SamplingPlan = labSample.SamplingPlan;
            sampleResult.UncertaintyPercent = labSample.UncertaintyPercent;

            return sampleResult;
        }

    }
}