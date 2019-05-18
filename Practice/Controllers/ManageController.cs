using Practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practice.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            var Profile = CustomerDataHandler.GetProfile(Models.Profile.Username);

            if (Profile != null)
            {
                ViewBag.Message = "welcome";
                return View(CustomerDataHandler.pobj);
            }
            else
                return RedirectToAction("Login", "Account", new { id = 3 });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //post /Manage/LogOff
        public ActionResult LogOff()
        {
            var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Account", new { id = 0 });
        }
    }
}