using System.Collections.Generic;
using System.Linq;

namespace Yay.Enumerations
{
	public class ConventionalEnumerationsOptionsProvider : IEnumerationOptionsProvider
	{
		public IEnumerable<EnumerationBase> Provide(EnumerationOptionsProviderContext context)
		{
			var enums = EnumerationHelper.GetAllNonDeprecated(context.EnumerationType).ToList();

			if (context.AttemptedValue != null && !enums.Contains(context.AttemptedValue))
			{
				enums = enums.Concat(context.AttemptedValue).ToList();
			}

			return enums;
		}
	}
}