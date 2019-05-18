using System.Web;
using System.Web.Mvc;
using Practice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace Practice.Controllers
{
    [Authorize(Roles = "User")]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Message = "welcome";
            return View();
        }

        // GET: Account/Login
        //public ActionResult Login()
        //{
        //    //if (!String.IsNullOrWhiteSpace(Message))
        //    //    ViewBag.Message = Message;
        //    return View();
        //}
        [AllowAnonymous]
        public ActionResult Login(int? id)
        {
            if (id == 0)
                ViewBag.Message = "";
            if (id == 1)
                ViewBag.Message = "Profile Created Successfully";
            else if (id == 2)
                ViewBag.Message = "Credentials is Invalid";
            else if(id == 3)
                ViewBag.Message = "Invalid Error Occured Try Login Again";
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // POST: Account/Verify
        public ActionResult Verify()
        {
            Models.Profile.Username = HttpContext.Request.Form["uname"].ToString();
            Models.Profile.Password = HttpContext.Request.Form["password"].ToString();
            Models.Profile.oldUsername = Models.Profile.Username;

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
                    return RedirectToAction("Login", "Account", new { id = 2 });
                }
                Profile();
                return View("Profile");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { id = 2 });
            }
        }
         
        [AllowAnonymous]
        [Authorize(Roles = "Admin, User")]
        public new ActionResult Profile()
        {
            var Profile = CustomerDataHandler.GetProfile(Models.Profile.Username);

            if (Profile != null)
            {
                ViewBag.Message = "welcome";
                return View(CustomerDataHandler.pobj);
            }
            else
                return RedirectToAction("Login", "Accounts", new { id = 3 });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create()
        {

            Profile umodel = new Profile
            {
                Full_Name = HttpContext.Request.Form["name"].ToString(),
                Dateofbirth = HttpContext.Request.Form["birthday"].ToString(),
                Gender = HttpContext.Request.Form["gender"].ToString(),
            };
            Models.Profile.Username = HttpContext.Request.Form["uname"].ToString();
            Models.Profile.Password = HttpContext.Request.Form["password"].ToString();
            Models.Profile.oldUsername = Models.Profile.Username;

            //return View("Login", "Profile Created Successfully");
            if (CustomerDataHandler.ChechUname(Models.Profile.Username))
            {
                if (CustomerDataHandler.Register(umodel))
                {
                    var userStore = new UserStore<IdentityUser>();
                    var manager = new UserManager<IdentityUser>(userStore);

                    var user = new IdentityUser() { UserName = Models.Profile.Username };
                    IdentityResult result = manager.Create(user, Models.Profile.Password);

                    if (result.Succeeded)
                    {

                        var _context = new ApplicationDbContext();
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                        await UserManager.AddToRoleAsync(user.Id, "User");

                        Verify();
                    }

                    if (!result.Succeeded)
                    {
                        ViewBag.Message = "Invalid Error Occured Try Again";
                        return View("Register");
                    }
                    return RedirectToAction("Login", "Account", new { id = 1 });
                }
                else
                {
                    ViewBag.Message = "Invalid Error Occured Try Again";
                    return View("Register");
                }
            }
            else
            {
                ViewBag.Message = "UserName Is Alredy Taken";
                return View("Register");
            }

            //in case user is being passed in without Id (unlikely), you could use user manager to get the full user object 
            //user = await this.UserManager.FindByNameAsync(user.UserName);

            //get all user's roles, and remove them
            
        }
   
        // POST : Account/SaveProfile
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> SaveProfile()
        {
            Profile umodel = new Profile();
            string msg = "";
            if (HttpContext.Request.Form["cpassword"].ToString() != "" && HttpContext.Request.Form["uname"].ToString() != "")
            {
                Models.Profile.oldUsername = Models.Profile.Username;
                if (Models.Profile.Username != HttpContext.Request.Form["uname"].ToString())
                {
                    Models.Profile.Username = HttpContext.Request.Form["uname"].ToString();
                    if (!CustomerDataHandler.ChechUname(Models.Profile.Username))
                        goto A;
                }
                if (Models.Profile.Password == HttpContext.Request.Form["cpassword"].ToString())
                {
                    if (HttpContext.Request.Form["newpassword"].ToString() != "")
                        if (HttpContext.Request.Form["confirm"].ToString() == HttpContext.Request.Form["newpassword"].ToString())
                        {
                            umodel.Passwords = Models.Profile.Password;
                            Models.Profile.Username = HttpContext.Request.Form["uname"].ToString();
                            Models.Profile.Password = HttpContext.Request.Form["newpassword"].ToString();
                        }
                    else
                        { msg = "Confirm and New Password Must Be Matched"; goto B; }
                }
                else
                { msg = "Current Password Must be Matched"; goto B; }
                umodel = new Profile
                {
                    Full_Name = HttpContext.Request.Form["name"].ToString(),
                    Dateofbirth = HttpContext.Request.Form["dateofbirth"].ToString(),
                    Email = HttpContext.Request.Form["email"].ToString(),
                    Locations = HttpContext.Request.Form["country"].ToString(),
                    Gender = HttpContext.Request.Form["gender"].ToString(),
                    Mobile = HttpContext.Request.Form["mobile"].ToString(),
                    Address = HttpContext.Request.Form["address"].ToString()
                };

                CustomerDataHandler ob = new CustomerDataHandler();
                if (await ob.SaveProfileAsync(umodel).ConfigureAwait(false))
                    msg = "Profile Updated Successfully";
                else
                    msg = "Unknown Error Occured Try Again Later";
            }
            else
            {
                Profile();
                ViewBag.Message = "Username & Current Password Must Be Filled";
                return View("Profile");
            }
        B:
            Profile();
            ViewBag.Message = msg;
            return View("Profile");
        A:
            msg = "Username Is Already Taken";
            goto B;
        }
    }
}