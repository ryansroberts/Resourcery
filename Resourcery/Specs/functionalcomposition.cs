using System;
using Machine.Specifications;
using Resourcery.FunctionalComposition;

namespace Resourcery.Specs
{
	public class make_function_with_passing_condition
	{
		static Func<string, string> func;

		Establish function_with_successful_condition = () =>
		  func = ConditionalExtensions.Chain<string,string>(() => "test",(s) => true);

		It executes = () => func("").ShouldEqual("test");
	}

	public class make_function_with_failing_condition
	{
		static Func<string, string> func;

		Establish function_with_failing_condition = () =>
		  func = ConditionalExtensions.Chain<string, string>(() => "test", (s) => false);

		It does_not_execute = () => func("").ShouldEqual(default(string));
	}

	public class chain_conditional_outer_fails
	{
		static Func<string, string> inner;

		static Func<string, string> func;

		Establish outer_with_failing_condition = () =>
		                                            {
		                                            	inner = ConditionalExtensions.Chain<string, string>
															(() => "test2",(s) => true);

		                                            	func = inner.Chain(s => false, () => "test1");
		                                            };

		It exececutes_inner_function = () => func("").ShouldEqual("test2");

	}

	public class chain_conditional_outer_succeeds
	{
		static Func<string, string> inner;
		static Func<string, string> func;

		Establish outer_with_failing_condition = () =>
		{
			inner = ConditionalExtensions.Chain<string, string>
				(() => "test2", (s) => true);

			func = inner.Chain(s => true, () => "test1");
		};

		It exececutes_outer_function = () => func("").ShouldEqual("test1");

	}


}