
using System.Web.Mvc;
using Website.Business;
using Website.Models;

namespace Website.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index(string name)
        {
            var viewModel = new ProfileViewModel {ProfileName = name};

            var metrics = new IndividualMetrics(name);

            viewModel.PopulateFastestRuns(metrics);
            viewModel.PopulateTotalDistances(metrics);
            viewModel.PopulateBestPaces(metrics);
            viewModel.PopulateTimeline(metrics);

            return View(viewModel);
        }

    }
}
