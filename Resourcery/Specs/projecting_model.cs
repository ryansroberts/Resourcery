using Machine.Specifications;

namespace Resourcery.Specs
{

	public class SimpleModel
	{
		public int Id { get; set; }
	}

	public class no_href_convention : with_resourcery<SimpleModel>
	{
		Because of_projecting_without_href_convention = () => project(new SimpleModel());

		It has_an_absolute_href_of_typename = () => projection.Intrinsics.Href.ShouldEqual("/simplemodel");

	}
}