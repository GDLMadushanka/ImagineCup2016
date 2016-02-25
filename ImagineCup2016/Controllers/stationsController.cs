using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImagineCup2016.Models;
using WebMatrix.WebData;
using System.Web.Security;
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
            data.ProducerList = new List<UserProfile>();

            //List<int> userids = db.webpages_Roles.Where(c => c.RoleId == 4).FirstOrDefault().UserProfiles.Select(x => x.UserId).ToList();


            //var stattionusers = from r in db.StationUsers
            //                    where userids.Contains((int)r.UserId)
            //                    & r.StationId == stationID
            //                    select r;

            var z = db.webpages_Roles.Where(c => c.RoleId == 4).First().UserProfiles.ToList(); // all announcer userprofiles
            var proUP = db.webpages_Roles.Where(c => c.RoleId == 3).First().UserProfiles.ToList(); // all producer userprofiles
            var y = db.StationUsers.Where(v => v.StationId == stationID).Select(b => b.UserId).ToList();// all users list of station list int
            data.AnnouncerList = z.Where(n => y.Contains(n.UserId)).ToList();
            data.ProducerList = proUP.Where(n => y.Contains(n.UserId)).ToList();



            station st = db.stations.Where(c => c.id == stationID).First();
            data.Station = st;
            List<Programme> plist = db.Programmes.Where(c => c.stationId == stationID).ToList();
            foreach (var item in plist)
            {
                Models.Programme p = new Models.Programme { moto = item.moto, name = item.name, StationId = item.stationId.Value };
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
