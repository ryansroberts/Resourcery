using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Resourcery.Model
{
	public class DynamicResource : DynamicObject
	{
		readonly Resource inner;


		public class LinkAccessor : DynamicObject
		{
			readonly IEnumerable<Link> links;

			public class ProjectedLink
			{
				public ProjectedLink(string href)
				{
					this.href = href;
				}

				public override string ToString()
				{
					return href;
				}

				public string href { get; protected set; }
			}

			public LinkAccessor(IEnumerable<Link> links)
			{
				this.links = links;
			}

			public override bool TryGetMember(GetMemberBinder binder, out object result)
			{
				result = null;
				var link = links.FirstOrDefault(l => l.Rel.Equals(binder.Name, StringComparison.InvariantCultureIgnoreCase));

				if( link != null)
				{
					result = new ProjectedLink(link.Href);
					return true;
				}

				return false;
			}
		}

		public abstract class ResourceAccessor<T> : DynamicObject, IEnumerable<DynamicResource> where T:Resource
		{
			readonly Func<T, string, bool> match;
			protected IEnumerable<T> resources;

			protected ResourceAccessor(Func<T,string,bool> match , IEnumerable<T> resources)
			{
				this.match = match;
				this.resources = resources;
			}

 
			public override bool TryGetMember(GetMemberBinder binder, out object result)
			{
				result = null;
				var resource = resources.Where(r => match(r, binder.Name)).ToArray();

				if (!resource.Any())
				{
					return false;
				}
                   
				if(resources.Count() == 1)
					result = new DynamicResource(resource.First());
				else
					result = resource.Select(r => new DynamicResource(r));

				return true;
			}

			public IEnumerator<DynamicResource> GetEnumerator()
			{
				return resources.Select(r => new DynamicResource(r)).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		public class EmbeddedAccessor : ResourceAccessor<EmbeddedResource>
		{
			public EmbeddedAccessor(IEnumerable<EmbeddedResource> resources) : base(
				(r,n) => r.Rel().Equals(n,StringComparison.InvariantCultureIgnoreCase)
				, resources)
			{
			}
		}

		public class FormAccessor : ResourceAccessor<Form>
		{
			public FormAccessor(IEnumerable<Form> resources)
				: base(
					(r, n) => r.Rel().Equals(n, StringComparison.InvariantCultureIgnoreCase)
					, resources)
			{
			}
		}

		public DynamicResource(Resource inner)
		{
			this.inner = inner;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (ProcessAsLinkAccessor(binder, out result)) return true;
			if (ProcessAsEmbeddedAccessor(binder, out result)) return true;
			if (ProcessAsFormAccessor(binder, out result)) return true;
			if (ProcessAsAttributeAccessor(binder, out result)) return true;

			return false;
		}

		bool ProcessAsAttributeAccessor(GetMemberBinder binder, out object result)
		{
			result = null;

			var attr = inner.Attributes().Properties().FirstOrDefault(
			                                                          p => p.Name.Equals(binder.Name, StringComparison.InvariantCultureIgnoreCase));

			if (attr != null)
			{
				if (attr.Value is string)
					result = attr.Value;

				if (attr.Value == null)
					result = "";

				return true;
			}
			return false;
		}

		bool ProcessAsEmbeddedAccessor(GetMemberBinder binder, out object result)
		{
			result = null;
			if(binder.Name == "_embedded")
			{
				result =  new EmbeddedAccessor(inner.Embedded());

				return true;
			}

			return false;
		}

		bool ProcessAsFormAccessor(GetMemberBinder binder, out object result)
		{
			result = null;
			if (binder.Name == "_forms")
			{
				result = new FormAccessor(inner.Forms());

				return true;
			}

			return false;
		}



		bool ProcessAsLinkAccessor(GetMemberBinder binder, out object result)
		{
			result = null;
			if (binder.Name == "_links")
			{
				result = new LinkAccessor(inner.Links());
				return true;
			}
			return false;
		}
	}
}