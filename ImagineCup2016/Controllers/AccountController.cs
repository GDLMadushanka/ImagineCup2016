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
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login logindata,String returnUrl)
        {
            if(WebSecurity.Login(logindata.UserName, logindata.Password, true)) {
                if (returnUrl != null)
                {
                    return RedirectToAction(returnUrl);
                }
                else
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Sorry invalid username or password");
                return View(logindata);
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register regdata,String role)
        {
           try
           {
                WebSecurity.CreateUserAndAccount(regdata.UserName, regdata.Password);
                Roles.AddUserToRole(regdata.UserName,role);
                return RedirectToAction("Index", "Home");
            }
            catch (System.Web.Security.MembershipCreateUserException ex)
            {
                ModelState.AddModelError("", "Username alredy exists");
                return View(regdata);
            }
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}