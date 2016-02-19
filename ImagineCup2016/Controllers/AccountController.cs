using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImagineCup2016.Models;
using WebMatrix.WebData;
using System.Web.Security;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ImagineCup2016.Controllers

{
    public class AccountController : Controller
    {
        private ImagineCupEntities db = new ImagineCupEntities();
        // GET: Account
     
        [HttpPost]
        public ActionResult Login(String UserName,String Password,String returnUrl)
        {
            TempUser temp = null;
                //db.TempUsers.Where(c => c.name == logindata.UserName && c.password == logindata.Password).First();

            if (temp==null && WebSecurity.Login(UserName,Password, true))
            {
                if (Roles.GetRolesForUser(UserName).Any())
                {
                    if (!(Roles.GetRolesForUser(UserName)[0].Equals("Administrator") || Roles.GetRolesForUser(UserName)[0].Equals("PhoneUser")))
                    {
                        int userid = db.UserProfiles.Where(m => m.UserName.Equals(UserName)).First().UserId;
                        int stationId = db.StationUsers.Where(c => c.UserId == userid).First().StationId.Value;
                        byte[] logo = db.stations.Where(c => c.id == stationId).First().logo;
                        Session["Logo"] = logo;
                        Session["UserId"] = userid;
                        Session["StationId"] = stationId;
                        return RedirectToAction("Index", "stations");
                    }
                    else return RedirectToAction("Index", "Home");
                }

                //if (returnUrl != null)
                //{
                //    return RedirectToAction(returnUrl);
                //}
                else
                    return RedirectToAction("Index", "Home");
            }
            else if (temp != null)
            {
                TempUserController con = new TempUserController();
                return RedirectToAction("Validate","TempUser",temp);
            }
            else
            {
                ModelState.AddModelError("", "Sorry invalid username or password");
                return View();
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register regdata)
        {

            var x = db.stations.Where(m => m.name.Equals(regdata.RadioStation.Name) || m.frequency.Equals(regdata.RadioStation.Frequency)).ToArray();

            if (x.Any())
            {
                ModelState.AddModelError("", "Station alredy exists");
                return View(regdata);
            }
            else {
                byte[] uploadedFile = new byte[regdata.RadioStation.Logo.InputStream.Length];
                regdata.RadioStation.Logo.InputStream.Read(uploadedFile, 0, uploadedFile.Length);


                Image img = Image.FromStream(regdata.RadioStation.Logo.InputStream);
                MemoryStream ms = new MemoryStream();

                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                {
                    img.Save(ms, ImageFormat.Jpeg);
                }
                else if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                {
                    img.Save(ms, ImageFormat.Gif);
                }
                else if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                {
                    img.Save(ms, ImageFormat.Bmp);
                }
                else if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                {
                    img.Save(ms, ImageFormat.Png);
                }

                try
                {
                    station temp = new station();
                    temp.approved = false;
                    temp.frequency = regdata.RadioStation.Frequency;
                    temp.name = regdata.RadioStation.Name;
                    temp.logo = ms.ToArray();

                    db.stations.Add(temp);
                    db.SaveChanges();
                    WebSecurity.CreateUserAndAccount(regdata.UserName, regdata.Password);
                    Roles.AddUserToRole(regdata.UserName, "StationAdmin");
                    

                    int StationId = db.stations.Where(m => m.name.Equals(temp.name)).First().id;
                    int userId = db.UserProfiles.Where(m => m.UserName.Equals(regdata.UserName)).First().UserId;

                    db.StationUsers.Add(new StationUser { StationId=StationId,UserId=userId});
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");

                }
                catch (System.Web.Security.MembershipCreateUserException ex)
                {
                    ModelState.AddModelError("", "Username alredy exists");
                    return View(regdata);
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult LoinTest()
        {
            return PartialView("_PartialLogin");
        }

    }
}