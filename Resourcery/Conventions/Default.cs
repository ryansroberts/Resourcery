namespace Resourcery.Conventions
{
	public static class Default
	{
		public static ConventionRule<ResourceContext,string> ResourceNameRule
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(c => c.Type.Name,c => true);
			}
		}

		public static ConventionRule<ResourceContext, string> RelNameRule
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(c => "self",c=> true);
			}
		}
	}
}