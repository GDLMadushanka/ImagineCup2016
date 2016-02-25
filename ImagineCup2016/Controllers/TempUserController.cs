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
        public JsonResult Create(Models.TempUser temp)
        {
            int stationid = (int)Session["StationId"];
            bool tempp = db.UserProfiles.Where(m =>m.UserName.Equals(temp.Name)).Any();
            if (tempp)
            {
                Response.StatusCode = 400;
                return Json(new { message = "This user already exists" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                WebSecurity.CreateUserAndAccount(temp.Name, temp.Password);
                if (temp.Role.Equals("Announcer"))
                {
                    Roles.AddUserToRole(temp.Name, "Announcer");
                }
                else if (temp.Role.Equals("Producer"))
                {

                    Roles.AddUserToRole(temp.Name, "Producer");
                }
                int userId = db.UserProfiles.Where(m => m.UserName.Equals(temp.Name)).First().UserId;
                db.StationUsers.Add(new StationUser { StationId = stationid, UserId = userId });
                db.SaveChanges();
                return Json(new { status = "1", message = "success" }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult newUserForm()
        {
            ImagineCup2016.Models.TempUser temp = new ImagineCup2016.Models.TempUser();
            return PartialView("_PartialCreateUser",temp);
        }
    }
}