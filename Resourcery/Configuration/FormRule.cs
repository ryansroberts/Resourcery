using System;

namespace Resourcery.Configuration
{
	public class FormRule : ResourceRule
	{
		public FormRule(Func<object, bool> match, Action<object, global::Resourcery.Model.Resource> action) : base(match, action)
		{
		}
	}
}