using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Models;
using Practice.ViewModel;
namespace Practice.Controllers
{
    public class PraController : Controller
    {
        public ViewResult Index()
        {
            var movies = GetMovies();

            return View(movies);
        }

        private IEnumerable<Datadb> GetMovies()
        {
            return new List<Datadb>
            {
                new Datadb { Id = 1, Name = "Shrek" },
                new Datadb { Id = 2, Name = "Wall-e" }
            };
        }
        // GET: Pra/Random
        public ActionResult Random()
        {
            var data = new Datadb() { Name = "Mateen" };
            var customers = new List<Profile>
            {
                //new Customer {Name = "c1"},
                //new Customer {Name = "c2"}
            };
            var viewmodel = new RandomClass
            {
                Movie = data,
                customer = customers
        };
            return View(viewmodel);
            // return Content("BCB");
            // Redirect("https://www.google.com");
        }
        public ActionResult Edit(int id)
        {
            return Content("ID = " + id);
        }
        //// Pra
        //public ActionResult Index(int PageNo, string SortBy)
        //{
        //    if (!PageNo.HasValue)
        //        PageNo = 1;
        //    if (String.IsNullOrWhiteSpace(SortBy))
        //        SortBy = "Name";
        //    return Content(String.Format("Page No = {0}, Sort BY = {1}",PageNo,SortBy));
        //}

        //Pra/wtf/year/

            [Route("Pra/wtf/{year:regex(\\d{4})}/{month}")]
        public ActionResult wtf(int year,int month)
        {
            return Content(year + "/" + month);
        }


    }
}