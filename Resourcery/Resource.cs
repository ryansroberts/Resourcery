using System;
using System.Dynamic;

namespace Resourcery
{
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
}