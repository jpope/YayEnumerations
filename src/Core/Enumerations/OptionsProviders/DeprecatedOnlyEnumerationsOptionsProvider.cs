using System.Collections.Generic;
using System.Linq;
using Yay.Enumerations;

namespace Core.Enumerations.OptionsProviders
{
	public class DeprecatedOnlyEnumerationsOptionsProvider : IEnumerationOptionsProvider
	{
		public IEnumerable<EnumerationBase> Provide(EnumerationOptionsProviderContext context)
		{
			var enums = EnumerationHelper.GetAll(context.EnumerationType).ToList();

			enums = enums.Where(x => x.DisplayName.Contains("Dep")).ToList();

			return enums;
		}
	}
}