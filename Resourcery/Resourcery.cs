using Resourcery.Conventions;

namespace Resourcery
{
	public class Resourcery
	{
		protected internal ConventionRules<ResourceContext,string> ResourceNameConventions = new ConventionRules<ResourceContext, string>();
		protected internal ConventionRules<ResourceContext, string> RelConventions = new ConventionRules<ResourceContext, string>();
		protected internal ConventionRules<ResourceContext,string> HrefConventions = new ConventionRules<ResourceContext, string>(); 

		public Resourcery()
		{
			ResourceNameConventions.Add(Default.ResourceNameRule);
			RelConventions.MustAlways(Default.RootRelIsAlwaysSelf);
			HrefConventions.Add(Default.HrefIsTypeName);
			HrefConventions.AddDecorator(Default.RootHrefIsAbsolute);
		}

		public Resource<T> Project<T>(T model)
		{
			return new Resource<T>(this,model);
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