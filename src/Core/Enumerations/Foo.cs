using Yay.Enumerations;

namespace Core.Enumerations
{
	public class Foo : Enumeration<Foo>
	{
		public static readonly Foo Bar = new Foo(1, "Foo");


		public Foo(int value, string displayName) : base(value, displayName)
		{
		}
	}
}
