using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Models;

namespace Practice.Controllers
{
    public class CustomerController : Controller
    {
        public ViewResult Index()
        {
            var customers = GetCustomers();

            return View(customers);
        }

        //public ActionResult Details(int id)
        //{
        //   // var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

        //    //if (customer == null)
        //    //    return HttpNotFound();

        //    //return View(customer);
        //}

        private IEnumerable<Profile> GetCustomers()
        {
            return new List<Profile>
            {
                //new Customer { Id = 1, Name = "John Smith" },
                //new Customer { Id = 2, Name = "Mary Williams" }
            };
        }
    }
}