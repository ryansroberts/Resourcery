using Resourcery.Configuration;

namespace Resourcery.Model
{
    public class Form : EmbeddedResource
    {
        public Form(ResourceRegistry registry, Resource parent, object originatingModel, string name) : base(registry, parent, originatingModel, name)
        {
        }
    }
}