using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Resourcery
{
	public abstract class Resource
	{
		public abstract string Name { get; }
		public abstract string Rel { get; }
		public abstract string Href { get; }
		public abstract IEnumerable<Resource> EmbeddedResources { get; }
		public abstract IEnumerable<Attribute> Attributes { get; }
	}

	public class Attribute
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public object Value { get; set; }

		public Attribute(string name, Type type, object value)
		{
			Name = name;
			Type = type;
			Value = value;
		}
	}

	public class Resource<TModel> : Resource
	{
		readonly Resourcery resourcery;
		readonly TModel model;


		public override string Name
		{
			get
			{
				return resourcery.ResourceNameConventions.MatchOne(ResourceContext.From(model,resourcery))();
			}
		}

		public override string Rel
		{
			get { return resourcery.RelConventions.MatchOne(ResourceContext.From(model,resourcery))(); }
		}

		public override string Href
		{
			get
			{
				return resourcery.HrefConventions.MatchOne(ResourceContext.From(model,resourcery))();
			}
		}

		public override IEnumerable<Resource> EmbeddedResources
		{
			get { return resourcery.EmbeddedResourceConventions.MatchAny(ResourceContext.From(model,resourcery))().SelectMany(s => s); }
		}

		public override IEnumerable<Attribute> Attributes
		{
			get 
			{
				return ResourceContext.From(model, resourcery).Attributes.Select(
					                                                          attribute =>
					                                                          resourcery.AttributeConventions.MatchOne(attribute)).
						Where(match => match != null)
						.Select(match => match());
			}
		}

		public IEnumerable<Form> Forms
		{
			get { return resourcery.FormConventions.MatchAny(ResourceContext.From(model, resourcery))().SelectMany(s => s); }
		}

		public Resource(Resourcery resourcery,TModel model)
		{
			this.resourcery = resourcery;
			this.model = model;
		}
	}
}