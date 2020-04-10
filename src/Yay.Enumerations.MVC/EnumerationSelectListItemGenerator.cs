using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yay.Enumerations.MVC
{
	public static class EnumerationSelectListItemGenerator
	{
		public static IEnumerable<SelectListItem> GenerateSelectListItems(EnumerationBase selectedEnum, IEnumerable<EnumerationBase> enumerations, bool insertBlank)
		{
			var items = new List<SelectListItem>();

			if (insertBlank)
			{
				items.Add(new SelectListItem {Text = String.Empty, Value = String.Empty});
			}

			items = items.Concat(enumerations
			    .Select(x => new SelectListItem
			        {
			            Text = x.DisplayName,
						Value = x.Value.ToString(),
						Selected = x.Equals(selectedEnum),
			        })
			).ToList();

			return items;
		}
	}
}