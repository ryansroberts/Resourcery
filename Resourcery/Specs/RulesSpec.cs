using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Resourcery.Configuration;

namespace Resourcery.Specs
{
	public class project_null_with_default_conventions
	{
		static ResourceRegistry registry;
		static ProjectionResult results;

		Establish context = () =>
		{
			registry = new ResourceRegistry();
		};

		Because of = () => results = registry.CreateResource(null);

		It has_no_links = () => results.Resource.Links().ShouldBeEmpty();

		It has_no_transitions = () => results.Resource.Links().ShouldBeEmpty();

		It has_no_attributes = () => results.Resource.Attributes().Properties().ShouldBeEmpty();

		[Ignore("Write diagnostics")]
		It has_self_error = () => results.Errors.ShouldContain("No link for self relation");
	}

	public class project_typed_null_with_default_conventions
	{
		static ResourceRegistry registry;
		static ProjectionResult results;

		Establish context = () =>
		{
			registry = new ResourceRegistry();
		};

		Because of = () => results = registry.CreateResource(null);

		It has_no_links = () => results.Resource.Links().ShouldBeEmpty();

		It has_no_transitions = () => results.Resource.Links().ShouldBeEmpty();

		It has_no_attributes = () => results.Resource.Attributes().Properties().ShouldBeEmpty();

		[Ignore("Write diagnostics")]
		It has_self_error = () => results.Errors.ShouldContain("No link for self relation");
	}

	public class simpleresource
	{
		public int Id { get; set; }
	}

	public class IdToLinkResourceRule : LinkRule
	{
		public IdToLinkResourceRule()
			: base(
				c => c != null && c.GetType().GetProperties().Any(p => p.Name == "Id") && ((dynamic)c).Id != null,
				(m, r) => r.AddLink(m.GetType().Name.ToLower() + "/" + ((dynamic)m).Id, "self"))
		{ }
	}

	public class attributed_resource
	{
		public string Id { get; set; }
		public string StringAttribute { get; set; }
		public List<string> CollectionAttribute { get; set; }
	}


	public class project_attributes
	{
		static ResourceRegistry registry;
		static ProjectionResult results;
		static dynamic dyn_resource()
		{
			return results.Resource;
		}

		Establish context = () =>
		{
			registry = new ResourceRegistry(c =>
			{
				c.Rule(new IdToLinkResourceRule());
				c.Rule(Model<attributed_resource>.HasAttributesFromProperties(a => true));
			});
		};

		Because of = () => results = registry.CreateResource(new attributed_resource() { StringAttribute = "value0" });

		It has_scalar_attribute = () => results.Resource.Attributes().Properties()
			 .Where(p => p.Name == "stringattribute").ShouldNotBeEmpty();

		It has_dynamic_attribute = () => ((string)dyn_resource().stringattribute).ShouldEqual("value0");

	}


	public class project_resource_with_self_convention
	{
		static ResourceRegistry registry;
		static ProjectionResult results;
		static dynamic dyn_resource()
		{
			return results.Resource;
		}

		Establish context = () =>
		{
			registry = new ResourceRegistry(
				c => c.Rule(new IdToLinkResourceRule())
				);
		};

		Because of = () => results = registry.CreateResource(new simpleresource { Id = 10 });

		It has_self_link_rel = () => results.Resource.Links().First().Rel.ShouldEqual("self");
		It has_self_link_href = () => results.Resource.Links().First().Href.ShouldEqual("simpleresource/10");

		It has_dynamic_link = () => ((string)dyn_resource()._links.self.href).ShouldEqual("simpleresource/10");
	}


	public class containeresource
	{
		public int Id { get; set; }
		public embeddedresource Single { get; set; }

		public List<embeddedresource> Many { get; set; }

	}


	public class resourcecollection
	{
		public string Id { get; set; }
		public List<embeddedresource> Collection { get; set; }
	}

	public class embeddedresource
	{
		public int Id { get; set; }
		public string AttributeEmbed { get; set; }
	}


	public class project_resource_with_embedded_resource
	{
		static ResourceRegistry registry;
		static ProjectionResult results;

		static dynamic dyn_resource()
		{
			return results.Resource;
		}


		Establish context = () =>
		{
			registry = new ResourceRegistry(
				c =>
				{
					c.Rule(new IdToLinkResourceRule());
					c.Rule(Model<containeresource>.Embed(m => m.Single, m => "single"));
					c.Rule(Model<containeresource>.EmbedMany(m => m.Many, m => "many"));
				});
		};

		Because of = () => results = registry.CreateResource(new containeresource
		{
			Id = 10,
			Single = new embeddedresource() { Id = 20 },
			Many = new[] { new embeddedresource() { Id = 30 } }.ToList()
		});

		It has_embedded_resources = () => results.Resource.Embedded().Count().ShouldEqual(2);

		It has_embedded_resource_links = () => results.Resource.Embedded()
			.SelectMany(r => r.Links()).Select(l => l.Href)
			.ShouldContain(new[] { "embeddedresource/20", "embeddedresource/30" });

		It has_dynamic_single_resource = () => ((object)dyn_resource()._embedded.single).ShouldNotBeNull();

		It has_dynamic_multiple_resource = () => ((IEnumerable)dyn_resource()._embedded.many).ShouldNotBeEmpty();

	}


	public class project_resource_with_forms
	{
		static ResourceRegistry registry;
		static ProjectionResult results;

		static dynamic dyn_resource()
		{
			return results.Resource;
		}


		Establish context = () =>
		{
			registry = new ResourceRegistry(
				c =>
				{
					c.Rule(NewDsl
						.Link(l =>
									{
					            		l.Href(m => m.GetType().Name.ToLower() + "/" + m.Id);
					            		l.Rel("self");
									})
						.Expression.Matches(m => m.Id != null)
						.Build());

	
					c.Rule(Model<resourcecollection>
						.EmbedMany(m => m.Collection, m => "collection"));
					c.Rule(Model<resourcecollection>.Post<embeddedresource>());
				});
		};

		Because of = () => results = registry.CreateResource(new resourcecollection(){Id = "1"});

		It has_self_link = () => ((string) dyn_resource()._links.self.href).ShouldEqual("resourcecollection/1");

		It has_post_form = () => results.Resource.Forms().Where(f => f.Rel() == "post")
									 .ShouldNotBeEmpty();

		It has_dynamic_post_form = () => ((object)dyn_resource()._forms.post).ShouldNotBeNull();

	}






}