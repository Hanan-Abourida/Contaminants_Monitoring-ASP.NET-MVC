using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Contaminants_Monitoring.Models;

namespace Contaminants_Monitoring.Controllers
{
    public class LabSamplesController : Controller
    {
        private FoodSafetyDBEntities db = new FoodSafetyDBEntities();

        // GET: LabSamples
        public ActionResult Index()
        {
            var labSamples = db.LabSamples.Include(l => l.Caza).Include(l => l.Commodity).Include(l => l.CommodityState).Include(l => l.ContaminantType).Include(l => l.Country).Include(l => l.Governorate).Include(l => l.Laboratory).Include(l => l.Mycotoxin).Include(l => l.PackagingType).Include(l => l.PesticideResidue).Include(l => l.SamplingReason).Include(l => l.SamplingType).Include(l => l.SampleOrigin);
            return View(labSamples.ToList());
        }

        // GET: LabSamples/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabSample labSample = db.LabSamples.Find(id);
            if (labSample == null)
            {
                return HttpNotFound();
            }
            return View(labSample);
        }

        // GET: LabSamples/Create
        public ActionResult Create()
        {
            ViewBag.fkCazaId = new SelectList(db.Cazas, "pkCazaId", "CazaCode");
            ViewBag.fkCommodityId = new SelectList(db.Commodities, "pkCommodityId", "CommodityCode");
            ViewBag.fkCommodityStateId = new SelectList(db.CommodityStates, "pkCommodityStateId", "CommodityDescription");
            ViewBag.fkContaminantTypeId = new SelectList(db.ContaminantTypes, "pkContaminantTypeId", "ContaminantName");
            ViewBag.fkOriginCountryId = new SelectList(db.Countries, "pkCountryId", "CountryName");
            ViewBag.fkGovernorateId = new SelectList(db.Governorates, "pkGovernorateId", "GovernorateCode");
            ViewBag.fkLaboratoryId = new SelectList(db.Laboratories, "pkLaboratoryId", "LaboratoryName");
            ViewBag.fkMycotoxinId = new SelectList(db.Mycotoxins, "pkMycotoxinId", "MycotoxinName");
            ViewBag.fkPackagingTypeId = new SelectList(db.PackagingTypes, "pkPackagingTypeId", "PackagingTypeCode");
            ViewBag.fkPesticideResidueId = new SelectList(db.PesticideResidues, "pkPesticideResidueId", "PestResName");
            ViewBag.fkSamplingReasonId = new SelectList(db.SamplingReasons, "pkSamplingReasonId", "SamplingReasonCode");
            ViewBag.fkSamplingTypeId = new SelectList(db.SamplingTypes, "pkSamplingTypeId", "SamplingTypeCode");
            ViewBag.fkSampleOriginId = new SelectList(db.SampleOrigins, "pkSampleOriginId", "OriginCode");
            return View();
        }

        // POST: LabSamples/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pkLabSampleId,SampleNb,LabSampleNb,OfficerName,OfficerPhoneNb,fkGovernorateId,fkCazaId,SamplingPlan,SampleCode,fkCommodityStateId,fkSamplingReasonId,fkSamplingTypeId,PremiseName,PremiseType,PremiseAddress,ContactName,ContactPhoneNumber,SamplingDate,SamplingTime,BrandName,Manufacturer,Distributor,Importer,fkSampleOriginId,fkOriginCountryId,fkPackagingTypeId,PackQuantitySize,BatchLotNumber,ShelfLife,StorageConditions,fkLaboratoryId,SampleQuantity,ReceivingDate,ReceivingTime,AnalysisDate,fkContaminantTypeId,fkPesticideResidueId,fkMycotoxinId,AnalysisPortion,AnalysisType,ConFinal,fkConUOM,LOD,LOQ,Uncertainty,UncertaintyPercent,ExtractionMethod,DeterminativeMethod,ViolationType,fkCommodityId")] LabSample labSample)
        {
            if (ModelState.IsValid)
            {
                db.LabSamples.Add(labSample);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fkCazaId = new SelectList(db.Cazas, "pkCazaId", "CazaCode", labSample.fkCazaId);
            ViewBag.fkCommodityId = new SelectList(db.Commodities, "pkCommodityId", "CommodityCode", labSample.fkCommodityId);
            ViewBag.fkCommodityStateId = new SelectList(db.CommodityStates, "pkCommodityStateId", "CommodityDescription", labSample.fkCommodityStateId);
            ViewBag.fkContaminantTypeId = new SelectList(db.ContaminantTypes, "pkContaminantTypeId", "ContaminantName", labSample.fkContaminantTypeId);
            ViewBag.fkOriginCountryId = new SelectList(db.Countries, "pkCountryId", "CountryName", labSample.fkOriginCountryId);
            ViewBag.fkGovernorateId = new SelectList(db.Governorates, "pkGovernorateId", "GovernorateCode", labSample.fkGovernorateId);
            ViewBag.fkLaboratoryId = new SelectList(db.Laboratories, "pkLaboratoryId", "LaboratoryName", labSample.fkLaboratoryId);
            ViewBag.fkMycotoxinId = new SelectList(db.Mycotoxins, "pkMycotoxinId", "MycotoxinName", labSample.fkMycotoxinId);
            ViewBag.fkPackagingTypeId = new SelectList(db.PackagingTypes, "pkPackagingTypeId", "PackagingTypeCode", labSample.fkPackagingTypeId);
            ViewBag.fkPesticideResidueId = new SelectList(db.PesticideResidues, "pkPesticideResidueId", "PestResName", labSample.fkPesticideResidueId);
            ViewBag.fkSamplingReasonId = new SelectList(db.SamplingReasons, "pkSamplingReasonId", "SamplingReasonCode", labSample.fkSamplingReasonId);
            ViewBag.fkSamplingTypeId = new SelectList(db.SamplingTypes, "pkSamplingTypeId", "SamplingTypeCode", labSample.fkSamplingTypeId);
            ViewBag.fkSampleOriginId = new SelectList(db.SampleOrigins, "pkSampleOriginId", "OriginCode", labSample.fkSampleOriginId);
            return View(labSample);
        }

        // GET: LabSamples/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabSample labSample = db.LabSamples.Find(id);
            if (labSample == null)
            {
                return HttpNotFound();
            }
            ViewBag.fkCazaId = new SelectList(db.Cazas, "pkCazaId", "CazaCode", labSample.fkCazaId);
            ViewBag.fkCommodityId = new SelectList(db.Commodities, "pkCommodityId", "CommodityCode", labSample.fkCommodityId);
            ViewBag.fkCommodityStateId = new SelectList(db.CommodityStates, "pkCommodityStateId", "CommodityDescription", labSample.fkCommodityStateId);
            ViewBag.fkContaminantTypeId = new SelectList(db.ContaminantTypes, "pkContaminantTypeId", "ContaminantName", labSample.fkContaminantTypeId);
            ViewBag.fkOriginCountryId = new SelectList(db.Countries, "pkCountryId", "CountryName", labSample.fkOriginCountryId);
            ViewBag.fkGovernorateId = new SelectList(db.Governorates, "pkGovernorateId", "GovernorateCode", labSample.fkGovernorateId);
            ViewBag.fkLaboratoryId = new SelectList(db.Laboratories, "pkLaboratoryId", "LaboratoryName", labSample.fkLaboratoryId);
            ViewBag.fkMycotoxinId = new SelectList(db.Mycotoxins, "pkMycotoxinId", "MycotoxinName", labSample.fkMycotoxinId);
            ViewBag.fkPackagingTypeId = new SelectList(db.PackagingTypes, "pkPackagingTypeId", "PackagingTypeCode", labSample.fkPackagingTypeId);
            ViewBag.fkPesticideResidueId = new SelectList(db.PesticideResidues, "pkPesticideResidueId", "PestResName", labSample.fkPesticideResidueId);
            ViewBag.fkSamplingReasonId = new SelectList(db.SamplingReasons, "pkSamplingReasonId", "SamplingReasonCode", labSample.fkSamplingReasonId);
            ViewBag.fkSamplingTypeId = new SelectList(db.SamplingTypes, "pkSamplingTypeId", "SamplingTypeCode", labSample.fkSamplingTypeId);
            ViewBag.fkSampleOriginId = new SelectList(db.SampleOrigins, "pkSampleOriginId", "OriginCode", labSample.fkSampleOriginId);
            return View(labSample);
        }

        // POST: LabSamples/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pkLabSampleId,SampleNb,LabSampleNb,OfficerName,OfficerPhoneNb,fkGovernorateId,fkCazaId,SamplingPlan,SampleCode,fkCommodityStateId,fkSamplingReasonId,fkSamplingTypeId,PremiseName,PremiseType,PremiseAddress,ContactName,ContactPhoneNumber,SamplingDate,SamplingTime,BrandName,Manufacturer,Distributor,Importer,fkSampleOriginId,fkOriginCountryId,fkPackagingTypeId,PackQuantitySize,BatchLotNumber,ShelfLife,StorageConditions,fkLaboratoryId,SampleQuantity,ReceivingDate,ReceivingTime,AnalysisDate,fkContaminantTypeId,fkPesticideResidueId,fkMycotoxinId,AnalysisPortion,AnalysisType,ConFinal,fkConUOM,LOD,LOQ,Uncertainty,UncertaintyPercent,ExtractionMethod,DeterminativeMethod,ViolationType,fkCommodityId")] LabSample labSample)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labSample).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fkCazaId = new SelectList(db.Cazas, "pkCazaId", "CazaCode", labSample.fkCazaId);
            ViewBag.fkCommodityId = new SelectList(db.Commodities, "pkCommodityId", "CommodityCode", labSample.fkCommodityId);
            ViewBag.fkCommodityStateId = new SelectList(db.CommodityStates, "pkCommodityStateId", "CommodityDescription", labSample.fkCommodityStateId);
            ViewBag.fkContaminantTypeId = new SelectList(db.ContaminantTypes, "pkContaminantTypeId", "ContaminantName", labSample.fkContaminantTypeId);
            ViewBag.fkOriginCountryId = new SelectList(db.Countries, "pkCountryId", "CountryName", labSample.fkOriginCountryId);
            ViewBag.fkGovernorateId = new SelectList(db.Governorates, "pkGovernorateId", "GovernorateCode", labSample.fkGovernorateId);
            ViewBag.fkLaboratoryId = new SelectList(db.Laboratories, "pkLaboratoryId", "LaboratoryName", labSample.fkLaboratoryId);
            ViewBag.fkMycotoxinId = new SelectList(db.Mycotoxins, "pkMycotoxinId", "MycotoxinName", labSample.fkMycotoxinId);
            ViewBag.fkPackagingTypeId = new SelectList(db.PackagingTypes, "pkPackagingTypeId", "PackagingTypeCode", labSample.fkPackagingTypeId);
            ViewBag.fkPesticideResidueId = new SelectList(db.PesticideResidues, "pkPesticideResidueId", "PestResName", labSample.fkPesticideResidueId);
            ViewBag.fkSamplingReasonId = new SelectList(db.SamplingReasons, "pkSamplingReasonId", "SamplingReasonCode", labSample.fkSamplingReasonId);
            ViewBag.fkSamplingTypeId = new SelectList(db.SamplingTypes, "pkSamplingTypeId", "SamplingTypeCode", labSample.fkSamplingTypeId);
            ViewBag.fkSampleOriginId = new SelectList(db.SampleOrigins, "pkSampleOriginId", "OriginCode", labSample.fkSampleOriginId);
            return View(labSample);
        }

        // GET: LabSamples/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabSample labSample = db.LabSamples.Find(id);
            if (labSample == null)
            {
                return HttpNotFound();
            }
            return View(labSample);
        }

        // POST: LabSamples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LabSample labSample = db.LabSamples.Find(id);
            db.LabSamples.Remove(labSample);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
