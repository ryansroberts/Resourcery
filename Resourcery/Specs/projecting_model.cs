using Machine.Specifications;
using Resourcery.Conventions;

namespace Resourcery.Specs
{

	public class SimpleModel
	{
		public int Id { get; set; }
	}

	public class no_href_convention : with_resourcery<SimpleModel>
	{
		Because of_projecting_without_href_convention = () => project(new SimpleModel());

		It has_an_absolute_href_of_typename = () => projection.Href.ShouldEqual("/simplemodel");
	}

	public class href_convention_for_type : with_resourcery<SimpleModel>
	{
		Establish href_for_simplemodel = () => resourcery.BuildHrefs.WhenResourceIsOfType(typeof(SimpleModel))
			.By(c => c.Type.Name.ToLower() + "/"  + ((dynamic)c.Instance).Id);

		Because of_projecting_with_href_convention = () => project(new SimpleModel(){Id = 12});

		It has_an_absolute_href_of_typename = () => projection.Href.ShouldEqual("/simplemodel/12");
	}


}