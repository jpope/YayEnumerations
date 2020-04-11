using System;
using System.Collections.Generic;

namespace Yay.Enumerations
{
	public interface IEnumerationOptionsProvider
	{
		IEnumerable<EnumerationBase> Provide(EnumerationOptionsProviderContext context);
	}

	public class EnumerationOptionsProviderContext
	{
		public Type EnumerationType { get; set; }
		public EnumerationBase AttemptedValue { get; set; }
	}
}