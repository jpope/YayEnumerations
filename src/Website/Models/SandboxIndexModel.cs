using System.ComponentModel.DataAnnotations;
using Core.Enumerations;
using Core.Enumerations.OptionsProviders;
using Yay.Enumerations.MVC.UiHints;

namespace Website.Models
{
	public class SandboxIndexModel
	{
		[DropDownList]
		public SandboxEnum SampleNormal { get; set; }
		[DropDownList]
		public SandboxEnum SampleDeprecated { get; set; }
		[DropDownList]
		public SandboxEnum SampleNoValue { get; set; }
		[DropDownList(InsertBlank = false)]
		public SandboxEnum SampleNoValNoBlank { get; set; }
		[DropDownList(InsertBlank = false, CustomProviderType = typeof(DeprecatedOnlyEnumerationsOptionsProvider))]
		public SandboxEnum SampleCustomDeprecatedOnly { get; set; }
	
		[RadioButtonList]
		[Required]
		public SandboxEnum RadiosSampleNormal { get; set; }
		[RadioButtonList]
		public SandboxEnum RadiosSampleDeprecated { get; set; }
		[RadioButtonList]
		public SandboxEnum RadiosSampleNoValue { get; set; }
		[RadioButtonList(RadioButtonsOrientation = RadioButtonsOrientation.Vertical)]
		public SandboxEnum RadiosSampleVertical { get; set; }
		[RadioButtonList(RadioButtonsOrientation = RadioButtonsOrientation.Horizontal)]
		public SandboxEnum RadiosSampleHorizontal { get; set; }
	}
}