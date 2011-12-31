using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace Resourcery.Specs
{
	public class ResourceWithAttributes
	{
		public ResourceWithAttributes() 
		{
			IntAttribute = 10;
			StringAttribute = "string";
		}

		public int IntAttribute { get; set; }
		public string StringAttribute { get; set; }
	}

	public class ResourceWithEnumerablePrimativeAttributes
	{
		public ResourceWithEnumerablePrimativeAttributes()
		{
			IntArray = new[]{1,2,3,4,5};
		}
		public IEnumerable<int> IntArray { get; set; }
	}

	[Ignore]
	public class enumerable_primative_attributes : with_resourcery<ResourceWithEnumerablePrimativeAttributes>
	{
		Because of = () => project(new ResourceWithEnumerablePrimativeAttributes());

		It has_int_array_attribute = () => projection.Attributes.First().Name.ShouldEqual("IntArray");

		It has_string_array_value = () => projection.Attributes.First().Value.ShouldEqual(new[] {"1", "2", "3", "4", "5"});

	}

	public class resource_attributes : with_resourcery<ResourceWithAttributes>
	{
		Because of = () => project(new ResourceWithAttributes());

		It has_int_attribute = () => has_attribute("IntAttribute", typeof (int), "10");
		It has_string_attribute = () => has_attribute("StringAttribute", typeof(string), "string");
		
		static void has_attribute(string name, Type type, string value)
		{
			projection.Attributes.ShouldNotBeEmpty();
			var attr = projection.Attributes.First(a => a.Name == name);
			attr.Type.ShouldEqual(type);
			attr.Value.ShouldEqual(value);
		}
	}



}