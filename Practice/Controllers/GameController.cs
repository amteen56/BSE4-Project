using System.Web.Mvc;
using System.Web.Services;
using Practice.Models;

namespace Practice.Controllers
{
    //[Authorize]
    public class GameController : Controller
    {
        public ActionResult Select()
        {
            ViewBag.Mesage = TempData["error"];
            return View();
        }
        [HttpPost]
        public ActionResult Selectuname(Users uname)
        {
            Users.rname = uname.Red_name;
            Users.gname = uname.Green_name;
            Users.bname = uname.Blue_name;
            Users.yname = uname.Yellow_name;
            if (GameDataHandler.addUsers(uname))
            {
                GameDataHandler.addPlay();
                return View("Play");
            }
            TempData["error"] = "Invalid Error Occured";
            return View("Select");
        }

        public ActionResult FeedBack()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        public ActionResult Report()
        {
            Feedback.username = HttpContext.Request.Form["Uname"].ToString();
            Feedback.Againstusername = HttpContext.Request.Form["auname"].ToString();
            Feedback.type = HttpContext.Request.Form["breport"].ToString();
            Feedback.Report = HttpContext.Request.Form["q4_describeFeedback"].ToString();
            if(GameDataHandler.Report())
            TempData["Message"] = "Thanks For Your FeedBack";
            else
                TempData["Message"] = "Invalid Error Occured Try Again";
            return View("FeedBack");
        }
        [WebMethod]
        public static void Winner(string color)
        {
            GameDataHandler.addWinner(color);
        }
    }
}