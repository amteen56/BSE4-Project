using System.Web;
using System.Web.Mvc;
using Practice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Practice.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        [AllowAnonymous]
        // GET: Admin/AdminLogin
        public ActionResult AdminLogin(int? id)
        {
            if (id == null)
                ViewBag.Message = "";
            else if (id == 2)
                ViewBag.Message = "Credentials is Invalid";
            else if (id == 3)
                ViewBag.Message = "Invalid Error Occured Try Login Again";
            return View();
        }
        public ActionResult Reports()
        {
            try
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            catch (System.Exception) { }
            return View(AdminDataHandler.GetFeedbackData());
        }
        public ActionResult BugReports()
        {
            try
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            catch (System.Exception) { }
            return View(AdminDataHandler.GetBugFeedbackData());
        }
        public ActionResult Records()
        {
            try
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            catch (System.Exception) { }
            return View(AdminDataHandler.GerUsersRecords());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // POST: Account/Verify
        public ActionResult Verify()
        {
            Models.Profile.Username = HttpContext.Request.Form["uname"].ToString();
            Models.Profile.Password = HttpContext.Request.Form["password"].ToString();

            if (CustomerDataHandler.Verify(Models.Profile.Username, Models.Profile.Password))
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = userManager.Find(Models.Profile.Username, Models.Profile.Password);

                if (user != null)
                {
                    var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                }
                else
                {
                    return RedirectToAction("AdminLogin", "Admin", new { id = 2 });
                }

                Accounts();
                return View("Account");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin", new { id = 2 });
            }
        }

        // GET: Admin/Subscriptions
        public ActionResult Accounts()
        {
            //ModelState.Clear();
            try
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            catch (System.Exception) { }
            return View(AdminDataHandler.GerUsersData());
        }

        public ActionResult AdminAccounts()
        {
            //ModelState.Clear();
            try
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            catch (System.Exception) { }
            return View(AdminDataHandler.GetAdminsData());
        }

        // GET: Admin/Subscriptions
        public ActionResult Subscriptions()
        {
            return View(AdminDataHandler.GerUsersData());
        }
        public ActionResult Resolve(int id)
        {
            AdminDataHandler.UpdateFeedBack(id);
            return RedirectToAction("Reports","Admin");
        }
        public ActionResult Deletefb(int id)
        {
            AdminDataHandler.DeleteFeedBack(id);
            return RedirectToAction("Reports", "Admin");
        }

        //GET: Admin/Edit/uname
        public ActionResult Edit(string id)
        {
            return View(AdminDataHandler.GerUsersData().Find(smodel => smodel.UserName == id));
        }
        public ActionResult Delete(string id)
        {
            if (AdminDataHandler.DeleteUser(id))
            {
                TempData["Message"] = "User Deleted Successfully";
            }
            else
                TempData["Message"] = "UnKnown Error Occured Try Again";
            return RedirectToAction("Subscriptions", AdminDataHandler.GerUsersData());
        }
        //POST: Admin/Edit/uname
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Profile profile)
        {
            try
            {
                if (AdminDataHandler.UpdateDetails(profile))
                    TempData["Message"] = "Profile Update Successfully";
                else
                    TempData["Message"] = "UnKnown Error Occured Try Again";
                return RedirectToAction("Subscriptions", AdminDataHandler.GerUsersData());
            }
            catch
            {
                ViewBag.Message = "UnKnown Error Occured Try Again";
                return View();
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(Profile profile)
        {
            try
            {
                if (CustomerDataHandler.ChechUname(Models.Profile.Username))
                {
                    if (AdminDataHandler.CreateUser(profile))
                    {
                        TempData["Message"] = "Profile Created Successfully";
                        var userStore = new UserStore<IdentityUser>();
                        var manager = new UserManager<IdentityUser>(userStore);

                        var user = new IdentityUser() { UserName = profile.UserName };
                        IdentityResult result = manager.Create(user, profile.Passwords);

                        if (result.Succeeded)
                        {

                            var _context = new ApplicationDbContext();
                            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                            await UserManager.AddToRoleAsync(user.Id, "User");
                        }

                        if (!result.Succeeded)
                        {
                            TempData["Message"] = "UnKnown Error Occured Try Again Make Sure You Filled All Details";
                            return RedirectToAction("Accounts", AdminDataHandler.GerUsersData());
                        }
                    }
                    else
                        TempData["Message"] = "UnKnown Error Occured Try Again Make Sure You Filled All Details";
                }
                else
                    TempData["Message"] = "UserName Already Taken";
                return RedirectToAction("Accounts", AdminDataHandler.GerUsersData());
            }
            catch
            {
                ViewBag.Message = "UnKnown Error Occured Try Again Make Sure You Filled All Details";
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> CreatenewAdmin(Profile profile)
        {
            try
            {
                if (CustomerDataHandler.ChechUname(Models.Profile.Username))
                {
                    if (AdminDataHandler.CreateUser(profile))
                    {
                        TempData["Message"] = "Profile Created Successfully";
                        var userStore = new UserStore<IdentityUser>();
                        var manager = new UserManager<IdentityUser>(userStore);

                        var user = new IdentityUser() { UserName = profile.UserName };
                        IdentityResult result = manager.Create(user, profile.Passwords);

                        if (result.Succeeded)
                        {

                            var _context = new ApplicationDbContext();
                            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                            await UserManager.AddToRoleAsync(user.Id, "Admin");
                        }

                        if (!result.Succeeded)
                        {
                            TempData["Message"] = "UnKnown Error Occured Try Again Make Sure You Filled All Details";
                            return RedirectToAction("AdminAccounts", AdminDataHandler.GerUsersData());
                        }
                    }
                    else
                        TempData["Message"] = "UnKnown Error Occured Try Again Make Sure You Filled All Details";
                }
                else
                    TempData["Message"] = "UserName Already Taken";
                return RedirectToAction("AdminAccounts", AdminDataHandler.GerUsersData());
            }
            catch
            {
                ViewBag.Message = "UnKnown Error Occured Try Again Make Sure You Filled All Details";
                return View();
            }
        }
    }

}