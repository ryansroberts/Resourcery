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

		public static ConventionRule<ResourceContext, string> RootRelIsAlwaysSelf
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(c => "self",c=> c.ParentContext == null);
			}
		}

		public static ConventionRule<ResourceContext,string> HrefIsTypeName
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(
						c => c.Type.Name.ToLower(), c => true
					);
			}
		}

		public static ConventionRuleDecorator<ResourceContext,string> RootResourceHasAbsoluteUri
		{
			get
			{
				return new ConventionRuleDecorator<ResourceContext, string>(
					   (c,u) => "/" + u,c => c.ParentContext == null
					);
			}
		}
	}
}