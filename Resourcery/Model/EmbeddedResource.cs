using Resourcery.Configuration;

namespace Resourcery.Model
{
	public class EmbeddedResource : Resource
	{
		protected Resource parent;
		protected string name { get; set; }

		public string Rel()
		{
			return name;
		}

		public Resource Parent()
		{
			return parent;
		}


		public EmbeddedResource(ResourceRegistry registry, 
		                        Resource parent, 
		                        object originatingModel, 
		                        string name)
			: base(registry, originatingModel)
		{

			this.parent = parent;
			this.name = name;
		}

	}
}