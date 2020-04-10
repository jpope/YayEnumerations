namespace Yay.Enumerations
{
	public abstract class OrderedEnumeration<T> : Enumeration<T> where T : OrderedEnumeration<T>
	{
		public int Order { get; private set; }

		protected OrderedEnumeration(int value, string displayName, int order) : base(value, displayName)
		{
			Order = order;
		}
	}
}
