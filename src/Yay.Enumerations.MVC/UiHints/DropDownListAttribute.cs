using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yay.Enumerations.MVC.UiHints
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DropDownListAttribute : UIHintAttribute
    {
    	public bool InsertBlank { get; set; }
    	public Type CustomProviderType { get; set; }
    	
		private IEnumerationOptionsProvider _provider;

    	public IEnumerationOptionsProvider Provider
    	{
    		get
    		{
				if (_provider == null)
					_provider = (IEnumerationOptionsProvider)Activator.CreateInstance(CustomProviderType);

    			return _provider;
    		}
    	}

    	public DropDownListAttribute() : base("EnumerationDropDownList")
		{
			InsertBlank = true;
    		CustomProviderType = typeof (ConventionalEnumerationsOptionsProvider);
		}

		public IEnumerable<SelectListItem> GetOptions(EnumerationBase attemptedValue, Type enumerationType)
		{
			var context = new EnumerationOptionsProviderContext
			{
				AttemptedValue = attemptedValue,
				EnumerationType = enumerationType
			};
			var enums = Provider.Provide(context);

			return EnumerationSelectListItemGenerator.GenerateSelectListItems(attemptedValue, enums, InsertBlank);
		}
    }
}