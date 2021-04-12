namespace Yay.Enumerations.UnitTests
{
    public class ForTestCasesEnumeration : Enumeration<ForTestCasesEnumeration>
    {
        public static readonly ForTestCasesEnumeration Normal = new ForTestCasesEnumeration(1, "Normal");
        public static readonly ForTestCasesEnumeration Normal2 = new ForTestCasesEnumeration(2, "Normal 2");
        [Deprecated]
        public static readonly ForTestCasesEnumeration Deprecated1 = new ForTestCasesEnumeration(11, "Dep 1");
        [Deprecated]
        public static readonly ForTestCasesEnumeration Deprecated2 = new ForTestCasesEnumeration(12, "Dep 2");

        public ForTestCasesEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}