using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Resourcery.Configuration;

namespace Resourcery.Model
{
	public class Resource : DynamicObject
    {
        readonly DynamicResource dynamicBehaviour;
        readonly object originatingModel;
        readonly ResourceRegistry registry;
        readonly List<Link> links = new List<Link>();
        readonly List<Form> forms = new List<Form>();
        readonly List<EmbeddedResource> embedded = new List<EmbeddedResource>();
        readonly Attributes attributes = new Attributes();

        public Resource(ResourceRegistry registry,object originatingModel)
        {
            dynamicBehaviour = new DynamicResource(this);
            this.registry = registry;
            this.originatingModel = originatingModel;
        }


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return dynamicBehaviour.TryGetMember(binder, out result);
        }

        public object Model()
        {
            return originatingModel;
        }

        public IEnumerable<EmbeddedResource> Embedded()
        {
            return embedded;
        }

        public IEnumerable<Link> Links()
        {
            return links;
        }

        public IEnumerable<Form> Forms()
        {
            return forms;
        }

        public Attributes Attributes()
        {
            return attributes;
        }


        public ProjectionResult From(object model,ResourceRegistry conventions)
        {
            var result =  conventions.CreateResource(model);
          
            if(!links.Any(l => l.Rel == "self"))
                result.Errors.Add("No link for self relation");
            
            return result;
        }

		public void AddLink(string href,string rel)
        {
            links.Add(new Link(href,rel));
        }
        
        public void AddEmbeddedResource(object model,string name)
        {
            var result = registry.CreateEmbeddedResource(model,this,name);
            embedded.Add(result);

        }

        public void AddForm(object model, string rel)
        {
            var result = registry.CreateForm(model, this, rel);

            forms.Add(result);
        }

        public void AddAttribute(string name, object value)
        {
            attributes.Add(name, value.ToString());
        }

       
        public Link Self()
        {
            return Links().FirstOrDefault(l => l.Rel == "self");
        }



       
    }
}