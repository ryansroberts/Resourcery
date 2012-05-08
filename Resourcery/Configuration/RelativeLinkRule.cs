using System;
using Resourcery.Model;

namespace Resourcery.Configuration
{
	public class RelativeLinkRule : LinkRule
	{
		public RelativeLinkRule(Func<object, bool> match, Action<object, EmbeddedResource> action)
			: base(match, (m, r) => action(m, (EmbeddedResource)r))
		{
		}
	}
}