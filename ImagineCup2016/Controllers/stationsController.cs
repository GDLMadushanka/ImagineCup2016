using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImagineCup2016;

namespace ImagineCup2016.Controllers
{
    public class stationsController : Controller
    {
        private ImagineCupEntities db = new ImagineCupEntities();

        // GET: stations
        public ActionResult Index()
        {
            int stationid = (int)Session["StationId"];
            Programme[] programes = db.Programmes.Where(c => c.stationId == stationid).ToArray();
            ViewBag.programmes = programes;
            return View();
        }


        // GET: stations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            station station = db.stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
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
