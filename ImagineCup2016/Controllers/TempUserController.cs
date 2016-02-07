using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImagineCup2016.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace ImagineCup2016.Controllers
{
    public class TempUserController : Controller
    {
        private ImagineCupEntities db = new ImagineCupEntities();
        public Models.TempUser tem;
        // GET: TempUser
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.TempUser temp)
        {
            int stationid = (int)Session["StationId"];
            TempUser tempuser = new TempUser { stationID = stationid, name = temp.Name, password = temp.Password, accountCreated = false,role=temp.Role };
            db.TempUsers.Add(tempuser);
            db.SaveChanges();
            return RedirectToAction("Index", "stations");
        }

        public ActionResult Validate(TempUser temp)
        {
            tem = new Models.TempUser{ Name = temp.name, Password = temp.password, stationId = temp.stationID.Value, Role = temp.role };
            ViewBag.temp = tem;
            return View();
        }
        [HttpPost]
        public ActionResult Validate2(Models.TempUser temp)
        {

                try
                {
                    WebSecurity.CreateUserAndAccount(temp.Name, temp.Password);
                    Roles.AddUserToRole(temp.Name,tem.Role);

                    int userId = db.UserProfiles.Where(m => m.UserName.Equals(temp.Name)).First().UserId;

                    db.StationUsers.Add(new StationUser { StationId = tem.stationId, UserId = userId });
                    db.SaveChanges();

                    return RedirectToAction("Login", "Account");

                }
                catch (System.Web.Security.MembershipCreateUserException ex)
                {
                    ModelState.AddModelError("", "Username alredy exists");
                    return View(temp);
                }
            }
    }
}