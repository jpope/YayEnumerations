using Core.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class SandboxController : Controller
    {
        public ActionResult Index()
        {
        	var model = new SandboxIndexModel
        	    {
					SampleNormal = SandboxEnum.Normal2,
					SampleDeprecated = SandboxEnum.Deprecated2,
					SampleNoValue = null,
					SampleNoValNoBlank = null,
					RadiosSampleNormal = SandboxEnum.Normal2,
					RadiosSampleNoValue = null,
					RadiosSampleDeprecated = SandboxEnum.Deprecated2
        	    };

			return View(model);
        }
		[HttpPost]
		public ActionResult Index(SandboxIndexModel model)
		{
			return View(model);
		}
    }
}
