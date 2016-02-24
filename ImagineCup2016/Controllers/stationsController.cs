using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImagineCup2016.Models;

namespace ImagineCup2016.Controllers
{
    public class stationsController : Controller
    {
        private ImagineCupEntities db = new ImagineCupEntities();

        // GET: stations
        public ActionResult Index(station st)
        {
            int stationid = (int)Session["StationId"];
            Programme[] programes = db.Programmes.Where(c => c.stationId == stationid).ToArray();
            ViewBag.programmes = programes;
            return View("Index");
        }

        public ActionResult StationDashboard()
        {

            int stationID = int.Parse(Session["StationId"].ToString());
            StationData data = new StationData();
            data.Station = new station();
            data.ProgramList = new List<Models.Programme>();
            data.ProducerList = new List<Models.Producer>();
            data.AnnouncerList = new List<Models.Announcer>();

            station st = db.stations.Where(c => c.id == stationID).First();
            data.Station = st;
            List<Programme> plist =db.Programmes.Where(c => c.stationId == stationID).ToList();
            foreach (var item in plist)
            {
                Models.Programme p = new Models.Programme {moto=item.moto,name=item.name,StationId=item.stationId.Value };
                data.ProgramList.Add(p);
            }
            return View(data);
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
