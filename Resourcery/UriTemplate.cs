using System;
using System.Collections.Generic;
using Machine.Specifications;

namespace Resourcery
{
	public interface ValueProvider
	{
		IDictionary<string, object> Values { get; set; }
	}

	public class AnonObjectValueProvider
	{
		
	}

	public class UriTemplate
	{
		string template;
		public UriTemplate(string s)
		{

			template = new Uri(NonEmpty(s), UriKind.Relative).ToString();
		}

		public UriTemplate(string somewhereParametised, object o)
		{
		}


		static string NonEmpty(string s)
		{
			return string.IsNullOrEmpty(s) ? "/" : s;
		}


		public override string ToString() { return template; }
	}


	public class empty_uri
	{
		static UriTemplate uri;

		Because of =()=>
		            {
		            	uri = new UriTemplate("");
		            };

		It is_relative_uri = () => uri.ToString().ShouldEqual("/");

	}

	public class constant_uri
	{
		static UriTemplate uri;

		Because of = () =>
		{
			uri = new UriTemplate("/somewhere/obvious");
		};

		It is_constant_uri = () => uri.ToString().ShouldEqual("/somewhere/obvious");
	}


	public class scalar_parameter_by_anonymous_type
	{
		static UriTemplate uri;

		Because of = () =>
		{
			uri = new UriTemplate("/somewhere/{parametised}", new { parametised  = 27});
		};

	}

}