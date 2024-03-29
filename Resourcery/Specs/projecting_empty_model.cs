﻿using System;
using Machine.Specifications;

namespace Resourcery.Specs
{
	public class with_resourcery<T>
	{
		protected static Resourcery resourcery;
		protected static Resource<T> projection;
		
		Establish service =()=> resourcery = new Resourcery();

		protected static void project(T model) { projection =  resourcery.Project<T>(model); }
	
	}

	public class projecting_empty_model : with_resourcery<SimplestPossibleModel>
	{
		Because of_projecting_empty_model = () => project(new SimplestPossibleModel());

		It has_resource = () => projection.ShouldNotBeNull();

		It resource_name_is_type_name = () => projection.Name.ShouldEqual(typeof (SimplestPossibleModel).Name);
	}

	public class resource_name_convention : with_resourcery<SimplestPossibleModel>
	{
		Establish context = () => resourcery.BuildResourceNames.Always().By((c) => "test");

		Because of_matching_conventionl = () => project(new SimplestPossibleModel());

		It has_resource_name_from_convention = () => projection.Name.ShouldEqual("test");
	}

	public class conditional_resource_name_convention : with_resourcery<SimplestPossibleModel>
	{
		Establish context = () => resourcery.BuildResourceNames.When(c => false).By((c) => "test");

		Because of_not_matching_conventionl = () => project(new SimplestPossibleModel());

		It has_resource_name_from_default_rule = () => projection.Name.ShouldEqual("SimplestPossibleModel");
	}


	public class resource_rel_convention : with_resourcery<SimplestPossibleModel>
	{
		Because of_projecting_with_defaults = () => project(new SimplestPossibleModel());

		It has_rel_of_self= () => projection.Rel.ShouldEqual("self");
	}

	public class resource_rel_convention_always_self_for_root : with_resourcery<SimplestPossibleModel>
	{
		Establish convention = () => resourcery.BuildRels.Always().By(c => "test");

		Because of_projecting_with_defaults = () => project(new SimplestPossibleModel());

		It has_rel_of_self = () => projection.Rel.ShouldEqual("self");
	}

}