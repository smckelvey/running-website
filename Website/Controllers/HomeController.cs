
using System.Web.Mvc;
using Website.Models;
using Website.Business;

namespace Website.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var viewModel = new HomeViewModel();

            var metrics = new RunningMetrics();

            viewModel.PopulateTotals(metrics.TotalYears, metrics.TotalRuns, metrics.TotalMiles, metrics.TotalHours);
            viewModel.PopulateNextRace(metrics.GetNextRace());
            viewModel.PopulateNextMarathon(metrics.GetNextMarathon());

            return View(viewModel);
        }
    }
}