using System;

namespace Resourcery.Conventions
{
	public class ConventionRule<ContextType, ResultType>
	{
		public Func<ContextType, ResultType> builder;
		public Func<ContextType, bool> condition;
		public ConventionRule(Func<ContextType, ResultType> builder, Func<ContextType, bool> condition)
		{
			this.builder = builder;
			this.condition = condition;
		}
	}
}