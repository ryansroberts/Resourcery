using System.Collections.Generic;

namespace Resourcery.Model
{
    public class Attributes
    {
        protected List<Property> properties = new List<Property>();

        public void Add(string name,object value)
        {
            properties.Add(new Property(name,value));
        }

        public IEnumerable<Property> Properties()
        {
            return properties;
        }

  
    }
}