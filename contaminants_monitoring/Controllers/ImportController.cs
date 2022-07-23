using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contaminants_Monitoring.Controllers
{
    public class ImportController : Controller
    {

        public ActionResult Import()
        {
            ImportSamplesViewModel importSamplesViewModel = new ImportSamplesViewModel();
            importSamplesViewModel.commodities = DataAccess.getAllCommodities();
            ViewBag.ShowError = false;
            ViewBag.ShowSuccess = false;
            return View(importSamplesViewModel);
        }

        [HttpPost]
        public ActionResult UploadExcel(ImportSamplesViewModel importSamplesViewModel, HttpPostedFileBase FileUpload)
        {

            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/App_Data/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }
                    //DataTable dt = con.GetSchema("Tables");
                    //string firstSheet = dt.Rows[0]["TABLE_NAME"].ToString();
                    //string secondSheet = dt.Rows[1]["TABLE_NAME"].ToString();
                    var adapter = new OleDbDataAdapter("SELECT * FROM [monitoring data record$]", connectionString);
                    var ds = new DataSet();
                    adapter.Fill(ds, "ExcelTable");
                    DataTable dtable = ds.Tables["ExcelTable"];
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);

                    MapExcelColumnsToLabSamples(excelFile);
                    var ExcelLabSamples = from excelRow in excelFile.Worksheet<LabSampleExcelMapping>(1) select excelRow;
                    if(!SaveExcelDataToDB(ExcelLabSamples, importSamplesViewModel.fkCommodityId))
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        ShowErrorMessage("Upload failed. One column or more is incompatible.");
                        importSamplesViewModel.commodities = DataAccess.getAllCommodities();
                        return View("Import", importSamplesViewModel);
                    }
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    ShowSuccessMessage();
                    importSamplesViewModel.commodities = DataAccess.getAllCommodities();
                    return View("Import", importSamplesViewModel);
                }
                else
                {
                    ShowErrorMessage("Only Excel file format is allowed.");
                    importSamplesViewModel.commodities = DataAccess.getAllCommodities();
                    return View("Import", importSamplesViewModel);
                }
            }
            else
            {
                ShowErrorMessage("Please choose Excel file");
                importSamplesViewModel.commodities = DataAccess.getAllCommodities();
                return View("Import",importSamplesViewModel);
            }
        }

        private bool SaveExcelDataToDB(IQueryable<LabSampleExcelMapping> ExcelLabSamples, int commodityId)
        {
            List<LabSample> labSamples = new List<LabSample>();
            foreach (var excelLabSample in ExcelLabSamples)
            {
                try
                {
                    if (excelLabSample.SampleNb != "")
                    {
                        LabSample newLabSample = new LabSample();
                        newLabSample.fkCommodityId = commodityId;
                        if (commodityId == 6)
                            newLabSample.fkCommodityStateId = 13;//Pistachios are always ready to eat
                        if (!String.IsNullOrEmpty(excelLabSample.CommDescription) || !String.IsNullOrWhiteSpace(excelLabSample.CommDescription))
                            newLabSample.fkCommodityStateId = DataAccess.GetCommodityStateId(commodityId, excelLabSample.CommDescription);

                        newLabSample.SampleNb = excelLabSample.SampleNb;
                        newLabSample.LabSampleNb = excelLabSample.LabSampleNb;
                        newLabSample.OfficerName = excelLabSample.OfficerName;
                        newLabSample.OfficerPhoneNb = excelLabSample.OfficerPhoneNb;
                        if (excelLabSample.AnalysisDate != null) newLabSample.AnalysisDate = DateTime.Parse(excelLabSample.AnalysisDate);
                        newLabSample.AnalysisPortion = excelLabSample.AnalysisPortion;
                        newLabSample.AnalysisType = excelLabSample.AnalysisType;
                        newLabSample.BatchLotNumber = excelLabSample.BatchLotNumber;
                        newLabSample.BrandName = excelLabSample.BrandName;
                        newLabSample.ContactName = excelLabSample.ContactName;
                        newLabSample.ContactPhoneNumber = excelLabSample.ContactPhoneNumber;
                        newLabSample.DeterminativeMethod = excelLabSample.DeterminativeMethod;
                        newLabSample.Distributor = excelLabSample.Distributor;
                        newLabSample.ExtractionMethod = excelLabSample.ExtractionMethod;
                        newLabSample.Importer = excelLabSample.Importer;
                        if (excelLabSample.LOD != null) newLabSample.LOD = Double.Parse(excelLabSample.LOD);
                        if (excelLabSample.LOQ != null) newLabSample.LOQ = Double.Parse(excelLabSample.LOQ);
                        newLabSample.Manufacturer = excelLabSample.Manufacturer;
                        newLabSample.PackQuantitySize = excelLabSample.PackQuantitySize;
                        newLabSample.PremiseName = excelLabSample.PremiseName;
                        newLabSample.PremiseType = excelLabSample.PremiseType;
                        newLabSample.PremiseAddress = excelLabSample.PremiseAddress;
                        if (excelLabSample.ReceivingDate != null) newLabSample.ReceivingDate = DateTime.Parse(excelLabSample.ReceivingDate);
                        newLabSample.ReceivingTime = excelLabSample.ReceivingTime;
                        newLabSample.SampleQuantity = excelLabSample.SampleQuantity;
                        if (excelLabSample.SamplingDate != null) newLabSample.SamplingDate = DateTime.Parse(excelLabSample.SamplingDate);
                        if(excelLabSample.SamplingPlanNb != null)
                            newLabSample.SamplingPlan = DataAccess.GetSamplingPlanIdFromText(excelLabSample.SamplingPlanNb);
                        newLabSample.SampleCode = excelLabSample.SampleCode;
                        newLabSample.SamplingTime = excelLabSample.SamplingTime;
                        newLabSample.ShelfLife = excelLabSample.ShelfLife;
                        newLabSample.StorageConditions = excelLabSample.StorageConditions;
                        if (excelLabSample.Uncertainty != null) newLabSample.Uncertainty = Double.Parse(excelLabSample.Uncertainty);
                        if (excelLabSample.UncertaintyPercent != null) newLabSample.UncertaintyPercent = Double.Parse(excelLabSample.UncertaintyPercent);

                        newLabSample.ConFinal = excelLabSample.ConFinal;

                        if(excelLabSample.GovernorateCode != null)
                            newLabSample.fkGovernorateId = DataAccess.GetGovernorateIdFromText(excelLabSample.GovernorateCode);
                        if (excelLabSample.Caza != null)
                            newLabSample.fkCazaId = DataAccess.GetCazaIdFromText(excelLabSample.Caza);
                        if (excelLabSample.SamplingReason != null)
                            newLabSample.fkSamplingReasonId = DataAccess.GetSamplingReasonIdFromText(excelLabSample.SamplingReason);
                        if (excelLabSample.SamplingType != null)
                            newLabSample.fkSamplingTypeId = DataAccess.GetSamplingTypeIdFromText(excelLabSample.SamplingType);
                        if (excelLabSample.Origin != null)
                            newLabSample.fkSampleOriginId = DataAccess.GetOriginIdFromText(excelLabSample.Origin);
                        if (excelLabSample.CountryOfOrigin != null)
                            newLabSample.fkOriginCountryId = DataAccess.GetCountryIdFromText(excelLabSample.CountryOfOrigin);
                        if (excelLabSample.PackagingType != null)
                            newLabSample.fkPackagingTypeId = DataAccess.GetPackagingTypeIdFromText(excelLabSample.PackagingType);
                        if (excelLabSample.LabCode != null)
                            newLabSample.fkLaboratoryId = DataAccess.GetLabIdFromText(excelLabSample.LabCode);

                        if (excelLabSample.Chemical != null)
                            newLabSample.fkContaminantTypeId = DataAccess.GetContaminantTypeIdFromText(excelLabSample.Chemical.Trim());

                        if (excelLabSample.ConUnit != null)
                            newLabSample.fkConUOM = DataAccess.GetUOMIdFromText(excelLabSample.ConUnit.Trim());

                        //
                        //Here is where we add code if more contaminants are defined in the system
                        //
                        if (excelLabSample.Chemical != null)
                        {
                            if (excelLabSample.Chemical.Trim() == "R")
                            {
                                if (excelLabSample.PestName != null && !excelLabSample.PestName.Contains("detected"))
                                {
                                    newLabSample.fkPesticideResidueId = DataAccess.GetPesticideResidueIdFromText(excelLabSample.PestName.Trim());
                                }
                                else newLabSample.fkPesticideResidueId = null;

                                //if(newLabSample.fkPesticideResidueId == -1)
                                //{
                                //    ShowErrorMessage("the file cotains new PR that you need to add to the database first");
                                //    return false;
                                //}
                                if (newLabSample.fkConUOM != 1) // == mg/kg
                                {
                                    //show error message for unaccepted unit of conFinal and the import will not be done
                                    return false;
                                }
                                //if (excelLabSample.ConFinal == "ND" || excelLabSample.ConFinal == "NQ" || excelLabSample.PestName.Contains("detec"))
                                //    newLabSample.ViolationType = "C";
                                //else newLabSample.ViolationType = DataAccess.SetResidueViolationType(Double.Parse(excelLabSample.ConFinal), commodityId, newLabSample.fkPesticideResidueId);
                                if (excelLabSample.ConFinal == "ND" || excelLabSample.ConFinal == "NQ" || excelLabSample.PestName.Contains("detec"))
                                    newLabSample.ComplianceResult = "C";
                                else if (DataAccess.ResidueIsBanned((int)newLabSample.fkPesticideResidueId))
                                {
                                    newLabSample.ComplianceResult = "V";
                                    newLabSample.ViolationType = "Banned Substance";
                                }
                                else
                                {
                                    newLabSample.ComplianceResult = DataAccess.SetResidueViolationType(Double.Parse(excelLabSample.ConFinal), commodityId, newLabSample.fkPesticideResidueId);
                                    if(newLabSample.ComplianceResult == "V") newLabSample.ViolationType = "Exceeding Regulatory Limit";
                                }
                            }

                            else if (excelLabSample.Chemical.Trim() == "AF")
                            {
                                newLabSample.fkMycotoxinId = DataAccess.GetMycotoxinIdFromText("Total Aflatoxins");
                                if (newLabSample.fkConUOM != 2 && newLabSample.fkConUOM != 6) // == mg/kg
                                {
                                    //show error message for unaccepted unit of conFinal and the import will not be done
                                    return false;
                                }
                                if (excelLabSample.ConFinal == "ND" || excelLabSample.ConFinal.Trim() == "NQ")
                                    newLabSample.ComplianceResult = "C";
                                else
                                {
                                    newLabSample.ComplianceResult = DataAccess.SetMycotoxinViolationType(Double.Parse(excelLabSample.ConFinal), commodityId, newLabSample.fkMycotoxinId);
                                    newLabSample.ViolationType = "Exceeding Regulatory Limit";
                                }
                            }
                        }

                        labSamples.Add(newLabSample);
                    }
                }
                catch (Exception e)
                {
                    Response.Write("You have error");
                    return false;
                }

            }
            if(DataAccess.AddNewLabSample(labSamples))
                return true;
            return false;
        }

        private void MapExcelColumnsToLabSamples(ExcelQueryFactory excelFile)
        {
            /** Mappings **/
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SampleNb, "Sample Nb");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.LabSampleNb, "Lab sample Nb (if diff)");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.OfficerName, "Sampling officer name");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.OfficerPhoneNb, "sampling officer phone number");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.GovernorateCode, "Governorate code");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Caza, "Caza");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SamplingPlanNb, "Sampling plan Nb");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SampleCode, "Sample code");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CommNb, "Comm Nb");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CommTypeCode, "Comm type code");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CommName, "Comm name");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CommDescription, "Comm description");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CommClass, "Comm class");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CommGroupCode, "Comm group code");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SamplingReason, "Sampling reason");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SamplingType, "Sampling type");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PremiseName, "Premise name");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PremiseType, "Premise type");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PremiseAddress, "Premise address");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ContactName, "contact person name");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ContactPhoneNumber, "contact person phone number");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SamplingDate, "Sampling date");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SamplingTime, "Sampling time");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.BrandName, "Brand name");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Manufacturer, "Manufacturer");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Distributor, "Distributor");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Importer, "Importer");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Origin, "Origin");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.CountryOfOrigin, "Country of Origin");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PackagingType, "type of packaging");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PackQuantitySize, "Pack quantity size");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.BatchLotNumber, "batch/lot number");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ShelfLife, "Shelf life");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.StorageConditions, "Storage conditions");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.LabCode, "Lab code");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.SampleQuantity, "sample quantity");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ReceivingDate, "Receiving date");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ReceivingTime, "Receiving time");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.AnalysisDate, "Analysis date");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Chemical, "Chemical");

            //Only For PR
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PestCode, "Pest code");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.PestName, "Pest name");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.TestClass, "TestClass");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.AnalysisPortion, "Analysis portion");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.AnalysisType, "Analysis type");

            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ConFinal, "Conc Final");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ConUnit, "Conc unit");


            excelFile.AddMapping<LabSampleExcelMapping>(x => x.LOD, "LOD");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.LOQ, "LOQ");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.Uncertainty, "UM");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.UncertaintyPercent, "UM%");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ExtractionMethod, "Extract");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.DeterminativeMethod, "Det");
            excelFile.AddMapping<LabSampleExcelMapping>(x => x.ViolationType, "Violation type");
        }

        public void ShowErrorMessage(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.ShowError = true;
            ViewBag.ShowSuccess = false;
        }

        public void ShowSuccessMessage()
        {
            ViewBag.ShowError = false;
            ViewBag.ShowSuccess = true;
        }


        public ActionResult DownloadAllImportTemplate()
        {
            string filename = "ImportAll_Template.xlsx";
            string filepath = Server.MapPath("~/App_Data/Templates/") + filename;
           // string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Path/To/File/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
    }

}