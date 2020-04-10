using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class GettingStartedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
		
		public ActionResult UsingNuget()
        {
            return View();
        }

    }
}
