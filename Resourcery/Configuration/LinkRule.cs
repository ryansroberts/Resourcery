using System;

namespace Resourcery.Configuration
{
	public class LinkRule : ResourceRule
	{
		public LinkRule(Func<object, bool> match, Action<object, global::Resourcery.Model.Resource> action)
			: base(match, action)
		{
		}

		public LinkRule() {}
	}
}