using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resourcery.Configuration
{
	public class AttributeRule : ResourceRule
	{   
		public class AttributeInfo
		{
			public AttributeInfo(string name, Type type, Type containingType, Func<object> valueSelector, Func<object> containingValueSelector)
			{
				Name = name;
				Type = type;
				ContainingType = containingType;
				ValueSelector = valueSelector;
				ContainingValueSelector = containingValueSelector;
			}

			public AttributeInfo(object container, PropertyInfo pinfo)
				: this(
					pinfo.Name.ToLower(), pinfo.PropertyType, container.GetType(),
					() => pinfo.GetValue(container, null), () => container)
			{ }
   

			public AttributeInfo(string name,object value)
			{
				Name = name.ToLower();
				ValueSelector = () => value;
			}


			public string Name;
			public Type Type;
			public Type ContainingType;
			public Func<object> ValueSelector;
			public Func<object> ContainingValueSelector;

		}


		public AttributeRule(Func<object, IEnumerable<AttributeInfo>> selector) : base(
			m => selector(m).Any(),
			(m,r) =>
			{
				foreach (var attributeInfo in selector(m))
					r.Attributes().Add(attributeInfo.Name, attributeInfo.ValueSelector());
			})
		{
		}

	}
}