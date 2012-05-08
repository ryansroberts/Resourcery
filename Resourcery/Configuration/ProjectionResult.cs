using System.Collections.Generic;

namespace Resourcery.Configuration
{
    public class ProjectionResult
    {
        public readonly List<string> Warnings = new List<string>();
        public readonly List<string> Errors = new List<string>();
        public readonly global::Resourcery.Model.Resource Resource;

        public ProjectionResult(global::Resourcery.Model.Resource resource)
        {
            Resource = resource;
        }
    }
}