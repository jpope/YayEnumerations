using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Enumerations;
using Core;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Yay.Enumerations;

namespace Website.Controllers
{
	public class EnumerationsController : Controller
    {
		[HttpGet]
        public ActionResult Index()
		{
			var assemblies = new[] {typeof(EmployeeType).Assembly};

			var model = assemblies
				.SelectMany(x => x.GetTypes())
				.Where(x => x.Closes(typeof (Enumeration<>)))
				.Select(GetEnumerationModel)
				.OrderBy(x => x.Name)
				.ToList();

			return View(model);
        }

		private static EnumerationModel GetEnumerationModel(Type type)
		{
			IEnumerable<PropertyInfo> properties = type.GetProperties();

			properties = properties.Where(x => x.Name == "Value")
				.Concat(properties.Where(x => x.Name == "DisplayName"))
				.Concat(properties.Where(x => x.Name != "Value" && x.Name != "DisplayName"));

			var allEnumerations = EnumerationHelper.GetAll(type);
			var allNonDeprecatedEnumerations = EnumerationHelper.GetAllNonDeprecated(type).ToList();

			var model = new EnumerationModel
			{
			    Name = type.Name,
			    PropertyNames = properties.Select(x => x.Name),
			    PropertyValues = allEnumerations.Select(x => new EnumerationPropertyModel
				{
					IsDeprecated = !allNonDeprecatedEnumerations.Contains(x),
					PropertyValues = properties.Select(y => GetDisplayValue(y.GetValue(x, null)))
				})
			};

			return model;
		}

		private static string GetDisplayValue(object value)
		{
			if (value is string)
			{
				return (string) value;
			}
			else if (value is IEnumerable)
			{
				return string.Join(", ", ((IEnumerable) value).Cast<object>().Select(GetDisplayValue));
			}
			else if (ReferenceEquals(value, null))
			{
				return string.Empty;
			}
			else
			{
				return value.ToString();
			}
		}
    }
}
