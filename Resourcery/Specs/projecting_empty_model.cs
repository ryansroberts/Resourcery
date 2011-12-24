using System;
using Machine.Specifications;

namespace Resourcery.Specs
{
	public class with_resourcery
	{
		protected static Resourcery resourcery;
		protected static Resource projection;
		
		Establish service =()=> resourcery = new Resourcery();

		protected static void project<T>(T model) { projection =  resourcery.Project(model); }
	
	}
	
	public class projecting_empty_model : with_resourcery
	{
		Because of_projecting_empty_model = () => project(new SimplestPossibleModel());

		It has_resource = () => projection.ShouldNotBeNull();

		It resource_name_is_type_name = () => projection.Intrinsics.Name.ShouldEqual(typeof (SimplestPossibleModel).Name);
	}

	public class resource_name_convention : with_resourcery
	{
		Establish context = () => resourcery.BuildResourceNames.Always().By((c) => "test");

		Because of_matching_conventionl = () => project(new SimplestPossibleModel());

		It has_resource_name_from_convention = () => projection.Intrinsics.Name.ShouldEqual("test");
	}

	public class conditional_resource_name_convention : with_resourcery
	{
		Establish context = () => resourcery.BuildResourceNames.When(c => false).By((c) => "test");

		Because of_not_matching_conventionl = () => project(new SimplestPossibleModel());

		It has_resource_name_from_default_rule = () => projection.Intrinsics.Name.ShouldEqual("SimplestPossibleModel");
	}

}