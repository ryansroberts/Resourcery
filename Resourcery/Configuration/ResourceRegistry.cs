using System;
using System.Collections.Generic;
using System.Linq;
using Resourcery.Model;

namespace Resourcery.Configuration
{
	public class ResourceRegistry
    {
        protected List<ResourceRule> resourceRules = new List<ResourceRule>();

       

        public ResourceRegistry() : this(c => {}) {}

        public ResourceRegistry(Action<ResourceRegistry> configure)
        {
            configure(this);
        }


        public ResourceRegistry Rule(AttributeRule rule)
        {
            resourceRules.Add(rule);

            return this;
        }


        public ResourceRegistry Rule(ResourceRule resourceRule)
        {
            resourceRules.Add(resourceRule);

            return this;
        }


        protected global::Resourcery.Model.Resource BuildStructure(global::Resourcery.Model.Resource resource,object model)
        {
        
            foreach (var embedRule in resourceRules.OfType<EmbedRule>().Where(r => r.Match(model)))
                embedRule.Apply(model, resource);

            foreach (var formRule in resourceRules.OfType<FormRule>().Where(r => r.Match(model)))
                formRule.Apply(model, resource);

            foreach (var attributeRule in resourceRules.OfType<AttributeRule>().Where(r => r.Match(model)))
                attributeRule.Apply(model, resource);
        
            return resource;
        }


        public EmbeddedResource CreateEmbeddedResource(object model, global::Resourcery.Model.Resource parent,string name)
        {
            var resource = new EmbeddedResource(this, parent,model,name);
            BuildStructure(resource, model);

            return resource;
        }

        public global::Resourcery.Model.Form CreateForm(object model, global::Resourcery.Model.Resource parent,string name)
        {
            var resource = new global::Resourcery.Model.Form(this, parent, model, name);
            BuildStructure(resource, model);

            return resource;
        }

        public ProjectionResult CreateResource(object model)
        {
            var resource = BuildStructure(new global::Resourcery.Model.Resource(this,model),model);


            ApplyLinkRules(model, resource);

            return new ProjectionResult(resource);
        }

        void ApplyLinkRules(object model, global::Resourcery.Model.Resource resource)
        {
           
            foreach (var linkRule in LinkRules()
                .Where(r => r.Match(model)))
                linkRule.Apply(model, resource);

            foreach (var embedded in resource.Embedded())
            {
                ApplyLinkRules(embedded.Model(),embedded);
            }
        }

        IEnumerable<LinkRule> LinkRules()
        {
            return resourceRules
                .OfType<LinkRule>();
        }
    }
}