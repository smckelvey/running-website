using System;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class CalendarController : BaseController
    {
        // GET: Calendar
        public ActionResult Index(string name, int? month = null, int? year = null)
        {
            //Translate request parameters
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (month != null && year != null)
            {
                startDate = new DateTime(year.Value, month.Value, 1);
            }

            //Build the page
            var viewModel = new CalendarViewModel();

            viewModel.Populate(name, startDate);

            return View(viewModel);
        }

        
    }
}