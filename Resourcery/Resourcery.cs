using Resourcery.Conventions;

namespace Resourcery
{
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