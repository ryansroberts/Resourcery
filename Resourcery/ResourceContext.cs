using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resourcery
{
	public class ResourceContext
	{
		public readonly Type Type;
		public readonly object Instance;
		public ResourceContext ParentContext;
		public readonly Resourcery Resourcery;

		public IEnumerable<ResourceAttributeContext> Attributes
		{
			get
			{
				return Type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => new ResourceAttributeContext(
					this, p.PropertyType, p.Name,p.GetValue(Instance,null)
				));
			}
		}

		protected ResourceContext(Type type, object instance,Resourcery resourcery)
		{
			Type = type;
			Instance = instance;
			Resourcery = resourcery;
		}

		public static ResourceContext From<T>(T model,Resourcery resourcery)
		{
			return new ResourceContext(typeof(T),model,resourcery);
		}
	}

	public class ResourceAttributeContext
	{
		public ResourceContext ResourceContext;
		public Type AttributeType;
		public string AttributeName;
		public object AttributeValue;

		public ResourceAttributeContext(ResourceContext resourceContext, Type attributeType, string attributeName,object attributeValue)
		{
			ResourceContext = resourceContext;
			AttributeType = attributeType;
			AttributeName = attributeName;
			AttributeValue = attributeValue;
		}

		public bool IsEnumerable()
		{
			return AttributeType.GetInterfaces()
				.Any(t => t.IsGenericType
						  && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
		}

		public bool IsValueTypeOrString() { return AttributeType.IsValueType || typeof(string).IsAssignableFrom(AttributeType); }
	}

}