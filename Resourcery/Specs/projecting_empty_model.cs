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
	
		protected static ResourceNameConvention resource_name_convention(Func<Type,object,string> convention)
		{
			return resourcery.BuildResourceTypeNameUsing(convention);
		}
	}
	
	public class projecting_empty_model : with_resourcery
	{
		Because of_projecting_empty_model = () => project(new SimplestPossibleModel());

		It has_resource = () => projection.ShouldNotBeNull();

		It resource_name_is_type_name = () => projection.Intrinsics.Name.ShouldEqual(typeof (SimplestPossibleModel).Name);
	}

	public class resource_name_convention : with_resourcery
	{
		Establish context = () => resource_name_convention((t, i) => "test");

		Because of_matching_conventionl = () => project(new SimplestPossibleModel());

		It has_resource_name_from_convention = () => projection.Intrinsics.Name.ShouldEqual("test");
	}

	public class conditional_resource_name_convention : with_resourcery
	{
		Establish context = () => resource_name_convention((t, i) => "test").When((t,i) => false);

		Because of_not_matching_conventionl = () => project(new SimplestPossibleModel());

		It has_resource_name_from_default_rule = () => projection.Intrinsics.Name.ShouldEqual("SimplestPossibleModel");
	}

}