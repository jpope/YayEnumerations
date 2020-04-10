using System.Collections.Generic;

namespace Website.Models
{
	public class EnumerationModel
	{
		public string Name { get; set; }
		public IEnumerable<string> PropertyNames { get; set; }
		public IEnumerable<EnumerationPropertyModel> PropertyValues { get; set; }
	}

	public class EnumerationPropertyModel
	{
		public bool IsDeprecated { get; set; }
		public IEnumerable<string> PropertyValues { get; set; }
	}
}