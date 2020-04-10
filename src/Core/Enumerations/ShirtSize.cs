using Yay.Enumerations;

namespace Core.Enumerations
{
	public class ShirtSize : OrderedEnumeration<ShirtSize>
	{
		public static readonly ShirtSize ExtraSmall = new ShirtSize(1, "XS", 1);
		public static readonly ShirtSize Small = new ShirtSize(2, "S", 2);
		public static readonly ShirtSize Medium = new ShirtSize(3, "M", 3);
		public static readonly ShirtSize Large = new ShirtSize(4, "L", 4);
		public static readonly ShirtSize ExtraLarge = new ShirtSize(5, "XL", 5);
		public static readonly ShirtSize ExtraExtraLarge = new ShirtSize(6, "XXL", 6);

		private ShirtSize(int value, string displayName, int order) : base(value, displayName, order) { }
	}
}