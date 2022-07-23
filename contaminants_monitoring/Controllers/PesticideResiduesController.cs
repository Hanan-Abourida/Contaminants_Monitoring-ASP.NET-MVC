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
    public class PesticideResiduesController : Controller
    {
        private FoodSafetyDBEntities db = new FoodSafetyDBEntities();

        // GET: PesticideResidues
        public ActionResult Index()
        {
            var pesticideResidues = db.PesticideResidues.Include(p => p.UOM).Include(p => p.UOM1);
            return View(pesticideResidues.ToList());
        }

        // GET: PesticideResidues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PesticideResidue pesticideResidue = db.PesticideResidues.Find(id);
            if (pesticideResidue == null)
            {
                return HttpNotFound();
            }
            return View(pesticideResidue);
        }

        // GET: PesticideResidues/Create
        public ActionResult Create()
        {
            ViewBag.fkADI_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit");
            ViewBag.fkARFD_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit");
            return View();
        }

        // POST: PesticideResidues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pkPesticideResidueId,PestResName,PestResCode,ADI,fkADI_UnitId,ARFD,fkARFD_UnitId,Reference,Banned")] PesticideResidue pesticideResidue)
        {
            if (ModelState.IsValid)
            {
                db.PesticideResidues.Add(pesticideResidue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fkADI_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit", pesticideResidue.fkADI_UnitId);
            ViewBag.fkARFD_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit", pesticideResidue.fkARFD_UnitId);
            return View(pesticideResidue);
        }

        // GET: PesticideResidues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PesticideResidue pesticideResidue = db.PesticideResidues.Find(id);
            if (pesticideResidue == null)
            {
                return HttpNotFound();
            }
            ViewBag.fkADI_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit", pesticideResidue.fkADI_UnitId);
            ViewBag.fkARFD_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit", pesticideResidue.fkARFD_UnitId);
            return View(pesticideResidue);
        }

        // POST: PesticideResidues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pkPesticideResidueId,PestResName,PestResCode,ADI,fkADI_UnitId,ARFD,fkARFD_UnitId,Reference,Banned")] PesticideResidue pesticideResidue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pesticideResidue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fkADI_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit", pesticideResidue.fkADI_UnitId);
            ViewBag.fkARFD_UnitId = new SelectList(db.UOMs, "pkUOMId", "unit", pesticideResidue.fkARFD_UnitId);
            return View(pesticideResidue);
        }

        // GET: PesticideResidues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PesticideResidue pesticideResidue = db.PesticideResidues.Find(id);
            if (pesticideResidue == null)
            {
                return HttpNotFound();
            }
            return View(pesticideResidue);
        }

        // POST: PesticideResidues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PesticideResidue pesticideResidue = db.PesticideResidues.Find(id);
            db.PesticideResidues.Remove(pesticideResidue);
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
