using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class VersionsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
