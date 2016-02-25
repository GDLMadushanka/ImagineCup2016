using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImagineCup2016.Controllers
{
    public class SetSessionController : Controller
    {
        // GET: SetSession
        public ActionResult SetVariable(string key, bool value)
        {
            Session[key] = value;

            return this.Json(new { success = true },JsonRequestBehavior.AllowGet);
        }
    }
}