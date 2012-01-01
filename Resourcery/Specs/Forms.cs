using Machine.Specifications;

namespace Resourcery.Specs
{
	public class UpdateableResource
	{
		public int SomeId { get; set; }
		public int SomeValue { get; set; }
	}

	public class adding_a_form_to_a_resource : with_resourcery<UpdateableResource>
	{
		Establish auto_form_projection = () => resourcery.Resource<UpdateableResource>()
		                                       	.HasForm("update")
		                                       	.FromType<UpdateableResource>()
												.UsesPut();

		Because project_resource_with_form = () => project(new UpdateableResource());

		It has_form = () => projection.Forms.ShouldNotBeEmpty();
	}
}