using Core.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Details()
        {
        	var model = new EmployeeDetailsModel
        		{
					FirstName = "Justin",
        			EmployeeType = EmployeeType.Servant,
        		};

        	return View(model);
        }
		
		public ActionResult Create()
        {
        	var model = new EmployeeCreateModel();

        	return View(model);
        }
		
		public ActionResult Edit(int id)
        {
        	var model = new EmployeeEditModel
        		{
					Id = id,
					FirstName = "Bob",
        			EmployeeType = EmployeeType.Slave,
        		};

        	return View(model);
        }
    }
}
