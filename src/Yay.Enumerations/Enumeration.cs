using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Yay.Enumerations
{
    [Serializable]
	[DebuggerDisplay("{DisplayName} - {Value}")]
	public abstract class Enumeration<T> : EnumerationBase, IComparable<T>, IEquatable<T>, IEnumeration where T : Enumeration<T>
	{
		private static readonly FieldInfo[] Fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
		private static readonly T[] AllEnumerations = Fields.Select(info => info.GetValue(null)).OfType<T>().ToArray();
		private static readonly T[] AllNonDeprecatedEnumerations = Fields.Where(info => !info.HasAttribute<DeprecatedAttribute>()).Select(info => info.GetValue(null)).OfType<T>().ToArray();

		protected Enumeration(int value, string displayName) : base(value, displayName)
		{
		}

		public static IEnumerable<T> GetAll()
		{
			return AllEnumerations;
		}

		public static IEnumerable<T> GetAllNonDeprecated()
		{
			return AllNonDeprecatedEnumerations;
		}

		public static T FromValue(int value)
        {
            return GetAll().SingleOrDefault(x => x.Value == value);
        }

		public static T FromDisplayName(string displayName)
		{
			var matchingItem = GetAll().FirstOrDefault(x => x.DisplayName == displayName);

			return matchingItem;
		}

		public static T FromProperty<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
		{
			var getPropertyValue = expression.Compile();

			var matchingItem = Find(value, expression.GetProperty().Name, x => getPropertyValue(x).Equals(value));

			return matchingItem;
		}

		private static T Find(object value, string description, Func<T, bool> predicate)
		{
            var enumerations = GetAll();
            var matchingItem = enumerations.FirstOrDefault(predicate);

			if (matchingItem == null)
			{
				var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));

				throw new ArgumentException(message, description);
			}

			return matchingItem;
		}

		public static T FromValueOrDefault(int value)
		{
			var matchingItem = GetAll().FirstOrDefault(x => x.Value == value);

			return matchingItem;
		}

		public static T FromDisplayNameOrDefault(string displayName)
		{
			var matchingItem = GetAll().FirstOrDefault(x => x.DisplayName == displayName);

			return matchingItem;
		}

		public override string ToString()
		{
			return DisplayName;
		}

		public int CompareTo(T other)
		{
			return ReferenceEquals(null, other) ? 1 : Value.CompareTo(other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (!(obj is Enumeration<T>)) return false;
			return Equals((T)obj);
		}

		public bool Equals(T other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.Value == Value;
		}

		public override int GetHashCode()
		{
			return Value;
		}

		public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
		{
			return left != null && right != null && left.Equals(right);
		}

		public static bool operator !=(Enumeration<T> left, Enumeration<T> right)
		{
			return !(left == right);
		}

		public static implicit operator string(Enumeration<T> enumeration)
		{
			return enumeration.DisplayName;
		}

		public static implicit operator int(Enumeration<T> enumeration)
		{
			return enumeration.Value;
		}
	}
}