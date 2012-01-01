using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Resourcery.Conventions
{

	public static class Extensions
	{
		public static ConventionBuilder<ResourceContext, TResult> WhenResourceIsOfType<TResult>(this ConventionRules<ResourceContext, TResult> rules, Type type)
		{
			return rules.When(c => type.IsAssignableFrom(c.Type));
		}

		 static ConventionRule<ResourceContext, IEnumerable<Resource>> MakeEmbedededResourceFor(PropertyInfo pInfo)
		 {
			 return new ConventionRule<ResourceContext, IEnumerable<Resource>>(
					c => new[] { c.Resourcery.ProjectEmbedded(pInfo.GetValue(c.Instance,null),c.Resource) },
					c => pInfo != null &&  c.Type == pInfo.DeclaringType && pInfo.GetValue(c.Instance,null) != null 
				 );
		 }

		public static Resourcery EmbeddedResourcesFrom<ResourceType>(this Resourcery resourcery, string propertyName)
		{
			resourcery.BuildEmbeddedResources.Add(MakeEmbedededResourceFor(typeof(ResourceType).GetProperty(propertyName,BindingFlags.Instance | BindingFlags.Public)));
			return resourcery;
		}

		public static Resourcery  EmbeddedResourcesFrom<ResourceType>(this Resourcery resourcery,Expression<Func<ResourceType,object>> expr)
		{
			return resourcery;
		}
	}
}