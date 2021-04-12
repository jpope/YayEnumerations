namespace Yay.Enumerations
{
    public abstract class EnumerationBase
    {
        public int Value { get; }

        protected EnumerationBase(int value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string DisplayName { get; }
    }
}