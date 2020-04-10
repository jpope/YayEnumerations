using Core.Enumerations;
using Yay.Enumerations.MVC.UiHints;

namespace Website.Models
{
	public class EmployeeDetailsModel
    {
		public string FirstName { get; set; }
		public EmployeeType EmployeeType { get; set; }
    }
    public class EmployeeCreateModel
    {
		public string FirstName { get; set; }
		[DropDownList]
		public EmployeeType EmployeeType { get; set; }
    }
    public class EmployeeEditModel
    {
		public int Id { get; set; }
		public string FirstName { get; set; }
		[DropDownList]
		public EmployeeType EmployeeType { get; set; }
    }
}
