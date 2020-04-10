using Yay.Enumerations;

namespace Core.Enumerations
{
	public class EmployeeType : Enumeration<EmployeeType>
	{
		public static readonly EmployeeType Manager = new EmployeeType(1, "Manager", 1000);
		public static readonly EmployeeType Servant = new EmployeeType(2, "Employee", 50);
		[Deprecated]
		public static readonly EmployeeType Slave = new EmployeeType(4, "Slave", 0);
		public static readonly EmployeeType AssistantManager = new EmployeeType(3, "Assistant Manager", 1400);

		public decimal Bonus { get; private set; }

		private EmployeeType(int value, string displayName, decimal bonus) 
			: base(value, displayName)
		{
			Bonus = bonus;
		}
	}
}