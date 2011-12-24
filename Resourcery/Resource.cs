using System;
using System.Dynamic;

namespace Resourcery
{
	public class Resource : DynamicObject
	{
		public class ResourceIntrinsics
		{
			readonly Func<string> resourceNameConvention;
			readonly Func<string> relConvention;

			public ResourceIntrinsics(Func<string> resourceNameConvention,Func<string> relConvention )
			{
				this.resourceNameConvention = resourceNameConvention;
				this.relConvention = relConvention;
			}

			public string Name { get { return resourceNameConvention(); } }

			public string Rel
			{
				get { return relConvention(); }
			}
		}

		public ResourceIntrinsics Intrinsics { get; protected set; }

		public Resource(Func<string> nameConvention,Func<string> relConvention)
		{
			Intrinsics = new ResourceIntrinsics(nameConvention,relConvention);
		}
	}
}