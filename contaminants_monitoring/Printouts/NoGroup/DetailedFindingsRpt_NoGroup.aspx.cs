using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Contaminants_Monitoring.Models.StatisticalAccess;

namespace Contaminants_Monitoring.Printouts
{
    public partial class DetailedFindingsRpt_NoGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ModelParam"] != null)
            {
                SearchViewModel model = (SearchViewModel)Session["ModelParam"];
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetailedFindings_NoGroup.rdlc");

                ReportViewer1.LocalReport.DisplayName = "Detailed_Findings" + "_" + DateTime.Now.ToString("yyyy-MM-dd");

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
               // ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing1);

                // paramaters to show the selected data filters
                ReportParameter SelectedCommodities;
                //if (model.allCommodities)
                //    SelectedCommodities = new ReportParameter("SelectedCommodities", "Apple - Pistachio");
                //else SelectedCommodities = new ReportParameter("SelectedCommodities", getSelectedCommoditiesString(model.selectedCommodities.ToList()));

                //ReportParameter SelectedPesticideResidues;
                //if (model.allPesticidesResidues)
                //    SelectedPesticideResidues = new ReportParameter("SelectedPestRes", "All Pesticides Residues");
                //else SelectedPesticideResidues = new ReportParameter("SelectedPestRes", getSelectedPesticideResiduesString(model.selectedPesticideResidues.ToList()));

                //ReportParameter SelectedMycotoxins;
                //if (model.allMycotoxins)
                //    SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", "All Mycotoxins");
                //else SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", getSelectedMycotoxinsString(model.selectedMycotoxins.ToList()));

                //ReportParameter SelectedYears;
                //if (model.allYears)
                //    SelectedYears = new ReportParameter("SelectedYears", "All Years");
                //else SelectedYears = new ReportParameter("SelectedYears", getSelectedYearsString(model.selectedYears.ToList()));
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
                else if (model.selectedMycotoxins != null)
                    SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", getSelectedMycotoxinsString(model.selectedMycotoxins.ToList()));
                else SelectedMycotoxins = new ReportParameter("SelectedMycotoxins", "");

                ReportParameter SelectedYears;
                if (model.selectedYears != null && model.selectedYears.Contains(0))
                    SelectedYears = new ReportParameter("SelectedYears", "All Years");
                else if (model.selectedYears != null)
                    SelectedYears = new ReportParameter("SelectedYears", getSelectedYearsString(model.selectedYears.ToList()));
                else SelectedYears = new ReportParameter("SelectedYears", "");

                ReportParameter finding_param = new ReportParameter("finding_param", model.detailedFindings.countPRsWithOneViolating.ToString());
                
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { SelectedCommodities, SelectedMycotoxins, SelectedPesticideResidues, SelectedYears, finding_param });
                ReportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource dataSource0 = new ReportDataSource("DataSet0", DataTableUtility.convertToDataTable(model.detailedFindings.atLeastOneDetectedMoleculeList));
                ReportDataSource dataSource1 = new ReportDataSource("DataSet3", DataTableUtility.convertToDataTable(model.detailedFindings.nonDetectedMoleculeList));
                ReportDataSource dataSource2 = new ReportDataSource("DataSet1", DataTableUtility.convertToDataTable(model.detailedFindings.quantifiedMoleculesList));
                ReportDataSource dataSource3 = new ReportDataSource("DataSet2", DataTableUtility.convertToDataTable(model.detailedFindings.nonQuantifiedMoleculeList));
                ReportDataSource dataSource4 = new ReportDataSource("DataSet4", DataTableUtility.convertToDataTable2(model.detailedFindings.commConClassifcations_violating));
                ReportDataSource dataSource5 = new ReportDataSource("DataSet5", DataTableUtility.convertToDataTable2(model.detailedFindings.commConClassifcations_unauthorized));
                ReportDataSource dataSource6 = new ReportDataSource("DataSet6", DataTableUtility.convertDictionaryToDataTable(model.detailedFindings.detectedPRForSpecificCommodity_all));
                ReportDataSource dataSource7 = new ReportDataSource("DataSet7", DataTableUtility.convertDictionaryToDataTable(model.detailedFindings.maxDetectedPRInSample));
                ReportDataSource dataSource8 = new ReportDataSource("DataSet8", DataTableUtility.convertCounterToDataTable(model.detailedFindings.samplesCounter_all));
                ReportDataSource dataSource9 = new ReportDataSource("DataSet9", DataTableUtility.convertToDataTable3(model.detailedFindings.ViolatingPerPR_all));
                ReportDataSource dataSource10 = new ReportDataSource("DataSet10", DataTableUtility.convertPRCounterToDataTable(model.detailedFindings.pRMoleculesDetected));

                ReportViewer1.LocalReport.DataSources.Add(dataSource0);
                ReportViewer1.LocalReport.DataSources.Add(dataSource1);
                ReportViewer1.LocalReport.DataSources.Add(dataSource2);
                ReportViewer1.LocalReport.DataSources.Add(dataSource3);
                ReportViewer1.LocalReport.DataSources.Add(dataSource4);
                ReportViewer1.LocalReport.DataSources.Add(dataSource5);
                ReportViewer1.LocalReport.DataSources.Add(dataSource6);
                ReportViewer1.LocalReport.DataSources.Add(dataSource7);
                ReportViewer1.LocalReport.DataSources.Add(dataSource8);
                ReportViewer1.LocalReport.DataSources.Add(dataSource9);
                ReportViewer1.LocalReport.DataSources.Add(dataSource10);
            }
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (Session["ModelParam"] != null)
            {
                SearchViewModel model = (SearchViewModel)Session["ModelParam"];
                // DataTable dataTable = DataTableUtility.ToDataTable<CommoditySeparatorDictionary>(model.detailedFindings.atLeastOneDetectedMoleculeList);
                DataTable dataTable = DataTableUtility.convertToDataTable(model.detailedFindings.atLeastOneDetectedMoleculeList);
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("DetailedFindings_NoGroup.rdlc");
                ReportDataSource dataSource = new ReportDataSource("DetailedFindingsDS", dataTable);
                e.DataSources.Add(dataSource);
                //ReportDataSource dataSource = new ReportDataSource("DetailedFindingsDs", getDataSourceValueFromMap(model));
            }
                
        }

        private void LocalReport_SubreportProcessing1(object sender, SubreportProcessingEventArgs e)
        {
            if (Session["ModelParam"] != null)
            {
                SearchViewModel model = (SearchViewModel)Session["ModelParam"];
                DataTable dataTable = DataTableUtility.convertToDataTable(model.detailedFindings.quantifiedMoleculesList);
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("quantifiedMoleculesList.rdlc");
                ReportDataSource dataSource = new ReportDataSource("quantifiedMoleculesList", dataTable);
                e.DataSources.Add(dataSource);
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
                    if (string.IsNullOrEmpty(paramStr))
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

        //public DataTable getDataSourceValueFromMap(SearchViewModel model)
        //{
        //    var table = new DataTable();
        //    var kvps = model.detailedFindings.atLeastOneDetectedMoleculeList.ToArray();
        //    table.Columns.AddRange(kvps.Select(kvp => new DataColumn(kvp.commodity)).ToArray());
        //    table.LoadDataRow(kvps.Select(kvp => kvp.samplesNbForChemical).ToArray(), true);
        //    return null;
        //}

        
    }
    public static class DataTableUtility
    {
        //public static DataTable ToDataTable<CommoditySeparatorDictionary>(this IList<CommoditySeparatorDictionary> data)
        //{
        //    PropertyDescriptorCollection properties =
        //        TypeDescriptor.GetProperties(typeof(CommoditySeparatorDictionary));
        //    DataTable table = new DataTable();
        //    foreach (PropertyDescriptor prop in properties)
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    foreach (CommoditySeparatorDictionary item in data)
        //    {
        //        DataRow row = table.NewRow();
        //        foreach (PropertyDescriptor prop in properties)
        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        table.Rows.Add(row);
        //    }
        //    return table;
        //}

        public static DataTable convertToDataTable(List<CommoditySeparatorDictionary> list)
        {
            DataTable dt = new DataTable("atLeastOneDetectedMoleculeList");
            dt.Columns.Add("commodity", typeof(string));
            dt.Columns.Add("chemical", typeof(string));
            dt.Columns.Add("number", typeof(string));
            dt.Columns.Add("percentage", typeof(string));

            foreach(CommoditySeparatorDictionary item in list)
            {
                foreach(var i in item.samplesNbForChemical.Keys)
                {
                    dt.Rows.Add(item.commodity,i, item.samplesNbForChemical[i].number, item.samplesNbForChemical[i].percentage + " % ");
                }
               
            }
            return dt;
        }

        public static DataTable convertToDataTable2(List<CommConClassifcation> list)
        {
            DataTable dt = new DataTable("atLeastOneDetectedMoleculeList");
            dt.Columns.Add("commodity", typeof(string));
            dt.Columns.Add("chemical", typeof(string));
            dt.Columns.Add("number", typeof(string));
            dt.Columns.Add("percentage", typeof(string));

            foreach (CommConClassifcation item in list)
            {
                dt.Rows.Add(item.commodity, item.contaminant, item.nbOfSamples, item.percentage + " % ");

            }
            return dt;
        }

        public static DataTable convertDictionaryToDataTable(Dictionary<string, int> map)
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("commodity", typeof(string));
            dt.Columns.Add("detected_PRs", typeof(string));

            foreach (var i in map.Keys)
            {
                dt.Rows.Add(i, map[i]);

            }
            return dt;
        }

        public static DataTable convertCounterToDataTable(List<SamplesCounter> list)
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("commodity", typeof(string));
            dt.Columns.Add("One_PR", typeof(string));
            dt.Columns.Add("One_PR_Per", typeof(string));
            dt.Columns.Add("Two_PR", typeof(string));
            dt.Columns.Add("Two_PR_Per", typeof(string));
            dt.Columns.Add("Three_to_Ten", typeof(string));
            dt.Columns.Add("Three_to_Ten_PR_Per", typeof(string));
            dt.Columns.Add("Ten_To_Max", typeof(string));
            dt.Columns.Add("Ten_To_Max_Per", typeof(string));

            foreach (var item in list)
            {
                dt.Rows.Add(item.commodity, item.OnePROnly,item.OnePROnly_percent,item.twoPROnly, item.twoPROnly_percent,item.threeToTenPR,item.threeToTenPR_percent,item.TenToMaxPR,item.TenToMaxPR_percent);

            }
            return dt;
        }

        public static DataTable convertToDataTable3(Dictionary<string, NumberPercentageVM> list)
        {
            DataTable dt = new DataTable("dt_");
            dt.Columns.Add("PR", typeof(string));
            dt.Columns.Add("NumberOfSamples", typeof(string));
            dt.Columns.Add("Percentage", typeof(string));

            foreach (var str in list.Keys)
            {
                    dt.Rows.Add(str, list[str].number, list[str].number + " % ");
            }
            return dt;
        }

        public static DataTable convertPRCounterToDataTable(PRMoleculesDetected item)
        {
            DataTable dt = new DataTable("dt_");
            dt.Columns.Add("one", typeof(string));
            dt.Columns.Add("two_to_ten", typeof(string));
            dt.Columns.Add("eleven_to_fifty", typeof(string));
            dt.Columns.Add("fiftyone_to_hdrd", typeof(string));
            dt.Columns.Add("hdrdOne_to_thsd", typeof(string));
            dt.Columns.Add("greater_than_thsd", typeof(string));
            
            dt.Rows.Add(item.molecules_1sample, item.molecules_2to10sample, item.molecules_11to50sample, item.molecules_51to100sample,item.molecules_101to1000sample, item.molecules_more1000sample);

            return dt;
        }
    }



}