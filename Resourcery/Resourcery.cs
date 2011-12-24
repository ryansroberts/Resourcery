using Resourcery.Conventions;

namespace Resourcery
{
	public class Resourcery
	{
		protected ConventionRules<ResourceContext,string> ResourceNameConventions = new ConventionRules<ResourceContext, string>();
		protected ConventionRules<ResourceContext, string> RelConventions = new ConventionRules<ResourceContext, string>();

		public Resourcery()
		{
			ResourceNameConventions.Add(Default.ResourceNameRule);
			RelConventions.MustAlways(Default.RelNameRule);
		}

		public Resource Project<T>(T model)
		{
			return new Resource(ResourceNameConventions.Match(new ResourceContext(typeof(T),model)),
					RelConventions.Match(new ResourceContext(typeof(T),model))
				);	
		}

		public ConventionRules<ResourceContext,string> BuildResourceNames
		{
			get { return ResourceNameConventions; }
		}

		public ConventionRules<ResourceContext, string> BuildRels
		{
			get { return RelConventions; }
		}
	}
}