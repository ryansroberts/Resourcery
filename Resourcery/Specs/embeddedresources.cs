using System.Linq;
using Machine.Specifications;
using Resourcery.Conventions;

namespace Resourcery.Specs
{
	public class RootModel
	{
		public RootModel()
		{
			EmbeddedRelation = new EmbeddedModel();
		}

		public class EmbeddedModel
		{
		
		}

		public EmbeddedModel EmbeddedRelation { get; set; }
	}

	
	public class embedded_scalar_resource : with_resourcery<RootModel>
	{
		Establish embeddedRule = () => resourcery
			.EmbeddedResourcesFrom<RootModel>(r => r.EmbeddedRelation);

		Because of = () => project(new RootModel());

		It has_embedded_resource = () => projection.EmbeddedResources.ShouldNotBeEmpty();

		It has_embedded_resource_with_rel_of_property_name = () =>
			projection.EmbeddedResources.Where(r => r.Rel == "embeddedrelation").ShouldNotBeEmpty();
	}
}