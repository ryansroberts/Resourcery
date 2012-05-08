using System;

namespace Resourcery.Configuration
{
	public class EmbedRule : ResourceRule {
		public EmbedRule(Func<object, bool> match, Action<object, global::Resourcery.Model.Resource> action) : base(match, action)
		{
		}

	}
}