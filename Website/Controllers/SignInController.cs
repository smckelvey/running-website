using System;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Password password)
        {
            if (ModelState.IsValid)
            {
                //TODO: aw come on, don't put the password straight into a cookie...
                HttpCookie cookie =
                    new HttpCookie("echelonx-Running-Authentication", password.Value)
                    {
                        Expires = DateTime.Now.AddDays(180)
                    };

                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            Response.Redirect("/RunningEvents");
            return null;
        }

    }
}