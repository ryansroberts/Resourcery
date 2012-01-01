using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Resourcery
{
	public abstract class Resource
	{
		protected Resource parent;
		protected object instance;

		public abstract string Name { get; }
		public abstract string Rel { get; }
		public abstract string Href { get; }
		public abstract IEnumerable<Resource> EmbeddedResources { get; }
		public abstract IEnumerable<Attribute> Attributes { get; }

		public Resource Parent
		{
			get { return parent; }
		}

		public object Instance
		{
			get { return instance; }
		}
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
		
		public override string Name
		{
			get
			{
				return resourcery.ResourceNameConventions.MatchOne(ResourceContext.From(this,resourcery,null))();
			}
		}

		public override string Rel
		{
			get { return resourcery.RelConventions.MatchOne(ResourceContext.From(this,resourcery,null))(); }
		}

		public override string Href
		{
			get
			{
				return resourcery.HrefConventions.MatchOne(ResourceContext.From(this,resourcery,null))();
			}
		}

		public override IEnumerable<Resource> EmbeddedResources
		{
			get { return resourcery.EmbeddedResourceConventions.MatchAny(ResourceContext.From(this,resourcery,null))().SelectMany(s => s); }
		}

		public override IEnumerable<Attribute> Attributes
		{
			get 
			{
				return ResourceContext.From(this, resourcery,null).Attributes.Select(
					                                                          attribute =>
					                                                          resourcery.AttributeConventions.MatchOne(attribute)).
						Where(match => match != null)
						.Select(match => match());
			}
		}

		public IEnumerable<Form> Forms
		{
			get { return resourcery.FormConventions.MatchAny(ResourceContext.From(this, resourcery,null))().SelectMany(s => s); }
		}

		public Resource(Resourcery resourcery,TModel model)
		{
			this.resourcery = resourcery;
			this.instance = model;
		}

		public Resource(Resourcery resourcery, TModel model, Resource parent)
		{
			this.resourcery = resourcery;
			this.instance = model;
			this.parent = parent;
		}
	}
}