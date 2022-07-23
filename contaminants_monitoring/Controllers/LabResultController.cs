using Contaminants_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contaminants_Monitoring.Controllers
{
    public class LabResultController : Controller
    {
        // GET: LabResult
        public ActionResult Index()
        {
            FoodSafetyDBEntities entities = new FoodSafetyDBEntities();
            List<LabSample> samples = entities.LabSamples.Where(l => l.Status == "Sent To Testing").ToList();

            //Add a Dummy Row.
            samples.Insert(0, new LabSample());
            return View(samples);
        }

        public ActionResult Index2()
        {
            FoodSafetyDBEntities entities = new FoodSafetyDBEntities();
            List<LabSample> samples = entities.LabSamples.Where(l => l.Status == "Sent To Testing").ToList();
            return View(samples);
        }

        [HttpPost]
        public JsonResult InsertCustomer(LabSample labSample)
        {
            using (FoodSafetyDBEntities entities = new FoodSafetyDBEntities())
            {
                entities.LabSamples.Add(labSample);
                entities.SaveChanges();
            }

            return Json(labSample);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(LabSample labSample)
        {
            using (FoodSafetyDBEntities entities = new FoodSafetyDBEntities())
            {
                LabSample updatedCustomer = (from c in entities.LabSamples
                                            where c.pkLabSampleId == labSample.pkLabSampleId
                                            select c).FirstOrDefault();
                updatedCustomer.ConFinal = labSample.ConFinal;
                updatedCustomer.fkPesticideResidueId = labSample.fkPesticideResidueId;
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int labSampleId)
        {
            using (FoodSafetyDBEntities entities = new FoodSafetyDBEntities())
            {
                LabSample labSample = (from c in entities.LabSamples
                                     where c.pkLabSampleId == labSampleId
                                      select c).FirstOrDefault();
                entities.LabSamples.Remove(labSample);
                entities.SaveChanges();
            }

            return new EmptyResult();
        }
    }
}