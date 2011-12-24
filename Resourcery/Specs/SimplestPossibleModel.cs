using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Resourcery.Conventions;
using Resourcery.FunctionalComposition;

namespace Resourcery
{
	public class SimplestPossibleModel
	{
	}

	public class ResourceContext
	{
		public readonly Type Type;
		public readonly object Instance;
		public ResourceContext ParentContext;

		public ResourceContext(Type type, object instance)
		{
			Type = type;
			Instance = instance;
		}
	}

	public class Resource : DynamicObject
	{
		public class ResourceIntrinsics
		{
			readonly Func<string> resourceNameConvention;

			public ResourceIntrinsics(Func<string> resourceNameConvention) {
				this.resourceNameConvention = resourceNameConvention;
			}

			public string Name { get { return resourceNameConvention(); } }
		}

		public ResourceIntrinsics Intrinsics { get; protected set; }

		public Resource(Func<string> nameConvention)
		{
			Intrinsics = new ResourceIntrinsics(nameConvention);
		}
	}

	public class Resourcery
	{
		protected ConventionRules<ResourceContext,string> ResourceNameConventions = new ConventionRules<ResourceContext, string>();

		public Resourcery()
		{
			ResourceNameConventions.Add(Default.ResourceNameRule);
		}

		public Resource Project<T>(T model)
		{
			return new Resource(ResourceNameConventions.Match(new ResourceContext(typeof(T),model)));	
		}

		public ConventionRules<ResourceContext,string> BuildResourceNames
		{
			get { return ResourceNameConventions; }
		}
	}
}
