using Yay.Enumerations;

namespace Core.Enumerations
{
	public class SandboxEnum : Enumeration<SandboxEnum>
	{
		public static readonly SandboxEnum Normal = new SandboxEnum(1, "Normal");
		public static readonly SandboxEnum Normal2 = new SandboxEnum(2, "Normal 2");
		[Deprecated]
		public static readonly SandboxEnum Deprecated1 = new SandboxEnum(11, "Dep 1");
		[Deprecated]
		public static readonly SandboxEnum Deprecated2 = new SandboxEnum(12, "Dep 2");

		public SandboxEnum(int value, string displayName) : base(value, displayName)
		{
		}
	}
}