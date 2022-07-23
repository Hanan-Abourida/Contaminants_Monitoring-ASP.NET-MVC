using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contaminants_Monitoring.Models;
using LinqToExcel;

namespace Contaminants_Monitoring.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //TO DO
            //Prepare dashboard components
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Import()
        {
            return RedirectToAction("Import", "Import");
        }

        public ActionResult SamplesCreation()
        {
            return RedirectToAction("SamplesCreation","Generate");
        }

        public ActionResult AllSamples()
        {
            return RedirectToAction("AllSamples", "Generate");
        }

        public ActionResult Map()
        {
            return RedirectToAction("Map", "Generate");
        }

        public ActionResult CollectionIndex()
        {
            return RedirectToAction("CollectionIndex", "Generate");
        }

        public ActionResult Search()
        {
            return RedirectToAction("Search", "Output");
        }

        public ActionResult CommodityHistory()
        {
            return RedirectToAction("CommodityHistory", "Output");
        }

        public ActionResult ContaminantHistory()
        {
            return RedirectToAction("ContaminantHistory", "Output");
        }

        public ActionResult NewLabSample()
        {
            return RedirectToAction("Create", "LabSamples");
        }

        public ActionResult ContaminantsLibrary()
        {
            return RedirectToAction("ContaminantsList", "Library");
        }

        public ActionResult Inventory()
        {
            return RedirectToAction("Inventory", "LabTest");
        }

        public ActionResult RiskTool()
        {
            return RedirectToAction("RiskToolFile","Output");
        }
    }
}