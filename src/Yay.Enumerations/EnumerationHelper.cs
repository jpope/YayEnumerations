using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yay.Enumerations
{
	public static class EnumerationHelper
	{
		public static IEnumerable<EnumerationBase> GetAll(Type type)
		{
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			return fields.Select(info => info.GetValue(null)).Cast<EnumerationBase>();
		}

		public static IEnumerable<T> GetAll<T>() where T : EnumerationBase
		{
			var type = typeof(T);

			return GetAll(type).OfType<T>();
		}

		public static IEnumerable<EnumerationBase> GetAllNonDeprecated(Type type)
		{
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
		
			var fieldsNotDeprecated = fields.Where(info => !info.HasAttribute<DeprecatedAttribute>());

			return fieldsNotDeprecated.Select(info => info.GetValue(null)).Cast<EnumerationBase>();
		}

		public static IEnumerable<T> GetAllNonDeprecated<T>() where T : EnumerationBase
		{
			var type = typeof(T);

			return GetAllNonDeprecated(type).OfType<T>();
		}
	}
}