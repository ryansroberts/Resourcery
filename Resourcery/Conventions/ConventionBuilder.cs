using System;

namespace Resourcery.Conventions
{
	public class ConventionBuilder<ContextType, ResultType>
	{
		readonly Func<ContextType, bool> condition;
		readonly Action<ConventionRule<ContextType, ResultType>> add;

		public ConventionBuilder(Func<ContextType, bool> condition, Action<ConventionRule<ContextType, ResultType>> add)
		{
			this.condition = condition;
			this.add = add;
		}

		public void By(Func<ContextType,ResultType> builder) 
		{ 
			add(new ConventionRule<ContextType, ResultType>(builder, condition)); 
		}
	}
}