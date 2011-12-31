using System;
using System.Linq.Expressions;

namespace Resourcery.Conventions
{
	public static class Extensions
	{
		public static ConventionBuilder<ResourceContext, TResult> WhenResourceIsOfType<TResult>(this ConventionRules<ResourceContext, TResult> rules, Type type)
		{
			return rules.When(c => type.IsAssignableFrom(c.Type));
		}

		public static Resourcery  EmbeddedResourcesFrom<ResourceType>(this Resourcery resourcery,Expression<Func<ResourceType,object>> expr) { return resourcery; }
	}
}