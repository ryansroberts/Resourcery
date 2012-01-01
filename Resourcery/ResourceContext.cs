using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resourcery
{
	public class ResourceContext
	{
		public readonly Type Type;
		public ResourceContext ParentContext;
		public readonly Resourcery Resourcery;
		public readonly Resource Resource;

		public  object Instance
		{
			get { return Resource.Instance; }
		}

		public IEnumerable<ResourceAttributeContext> Attributes
		{
			get
			{
				return Type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => new ResourceAttributeContext(
					this, p.PropertyType, p.Name,p.GetValue(Instance,null)
				));
			}
		}

		
		ResourceContext(Resource resource,Type type, ResourceContext parentContext, Resourcery resourcery)
		{
			Resource = resource;
			Type = type;
			ParentContext = parentContext;
			Resourcery = resourcery;
		}


		public static ResourceContext From(Resource resource,Resourcery resourcery,ResourceContext parent)
		{
			if (resource == null) return null;

			return new ResourceContext(resource,resource.Instance.GetType(),parent,resourcery);
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