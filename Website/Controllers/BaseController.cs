
using System.Configuration;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class BaseController : Controller
    {

    }

    public class SimpleMembershipAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if not authenticated
            var cookie = filterContext.HttpContext.Request.Cookies["echelonx-Running-Authentication"];

            //TODO: this is terrible, obviously
            var systemPassword = ConfigurationManager.AppSettings["SystemPassword"];
            var isAuthenticated = cookie != null && cookie.Value == systemPassword;

            if (!isAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect("/", true);
            }
        }
    }
}