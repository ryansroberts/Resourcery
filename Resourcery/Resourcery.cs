using System.Collections.Generic;
using Resourcery.Conventions;

namespace Resourcery
{

	public class Form
	{
		public Resource Resource { get; set; }
		public string Method { get; set; }
	}

	public class FormBuilder<T>
	{
		readonly ConventionRules<ResourceContext, IEnumerable<Form>> formConventions;

		public FormBuilder(ConventionRules<ResourceContext, IEnumerable<Form>> formConventions) {
			this.formConventions = formConventions;
		}

		public FormNameBuilder<T> HasForm(string name) { return new FormNameBuilder<T>(formConventions); }

	}

	public class FormNameBuilder<T>
	{
		readonly ConventionRules<ResourceContext, IEnumerable<Form>> formConventions;

		public FormNameBuilder(ConventionRules<ResourceContext, IEnumerable<Form>> formConventions) {
			this.formConventions = formConventions;
		}

		public FormMethodBuilder<T> FromType<TForm>() { return new FormMethodBuilder<T>(formConventions); }
	
	}

	public class FormMethodBuilder<T>
	{
		readonly ConventionRules<ResourceContext, IEnumerable<Form>> formConventions;

		public FormMethodBuilder(ConventionRules<ResourceContext, IEnumerable<Form>> formConventions) {
			this.formConventions = formConventions;
		}

		public void UsesPut() { }
		public void UsesPost() { }
		public void UsesPatch() {}

	}


	public class Resourcery
	{
		protected internal ConventionRules<ResourceContext,string> ResourceNameConventions = new ConventionRules<ResourceContext, string>();
		protected internal ConventionRules<ResourceContext, string> RelConventions = new ConventionRules<ResourceContext, string>();
		protected internal ConventionRules<ResourceContext,string> HrefConventions = new ConventionRules<ResourceContext, string>();
		protected internal ConventionRules<ResourceContext, IEnumerable<Resource>> EmbeddedResourceConventions = new ConventionRules<ResourceContext, IEnumerable<Resource>>(); 
		protected internal ConventionRules<ResourceAttributeContext,Attribute> AttributeConventions  = new ConventionRules<ResourceAttributeContext, Attribute>();
		protected internal ConventionRules<ResourceContext,IEnumerable<Form>> FormConventions = new ConventionRules<ResourceContext, IEnumerable<Form>>(); 

		public Resourcery()
		{
			ResourceNameConventions.Add(Default.ResourceNameRule);
			RelConventions.MustAlways(Default.RootRelIsAlwaysSelf);
			HrefConventions.Add(Default.HrefIsTypeName);
			HrefConventions.AddDecorator(Default.RootResourceHasAbsoluteUri);
			AttributeConventions.Add(Default.SimpleTypeAttributes);
		}

		public Resource<T> Project<T>(T model)
		{
			return new Resource<T>(this,model);
		}

		public Resource<T> ProjectEmbedded<T>(T model,Resource parent)
		{
			return new Resource<T>(this, model,parent);
		}


		public ConventionRules<ResourceContext,string> BuildResourceNames
		{
			get { return ResourceNameConventions; }
		}

		public ConventionRules<ResourceContext, string> BuildRels
		{
			get { return RelConventions; }
		}

		public ConventionRules<ResourceContext, string> BuildHrefs
		{
			get { return HrefConventions; }
		}

		public ConventionRules<ResourceContext, IEnumerable<Resource>> BuildEmbeddedResources
		{
			get { return EmbeddedResourceConventions; }
		}

		public ConventionRules<ResourceAttributeContext, Attribute> BuildAttributes
		{
			get { return AttributeConventions; }
		}

		public FormBuilder<T>  Resource<T>()
		{
				return new FormBuilder<T>(FormConventions);
		}
	}
}