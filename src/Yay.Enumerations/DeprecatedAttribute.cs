using System;

namespace Yay.Enumerations
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DeprecatedAttribute : Attribute { }
}