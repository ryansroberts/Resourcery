using System;
using System.Dynamic;

namespace Resourcery
{
	public class Resource<TModel> : DynamicObject
	{
		readonly Resourcery resourcery;
		readonly TModel model;

		public class ResourceIntrinsics<TModel>
		{
			readonly Resourcery resourcery;
			readonly TModel model;

			public ResourceIntrinsics(Resourcery resourcery,TModel model)
			{
				this.resourcery = resourcery;
				this.model = model;
			}

			public string Name 
			{
				get 
				{
					return resourcery.ResourceNameConventions.Match(ResourceContext.From(model))();
				} 
			}

			public string Rel
			{
				 get { return resourcery.RelConventions.Match(ResourceContext.From(model))(); } 
			}

			public string Href
			{
				get 
				{
					return resourcery.HrefConventions.Match(ResourceContext.From(model))();
				}
			}
		}

		public ResourceIntrinsics<TModel> Intrinsics
		{
			get
			{
				return new ResourceIntrinsics<TModel>(resourcery,model);
			}
		}

		public Resource(Resourcery resourcery,TModel model)
		{
			this.resourcery = resourcery;
			this.model = model;
		}
	}
}