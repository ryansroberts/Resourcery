using System;
using System.Linq.Expressions;

namespace Resourcery.Conventions
{
	public class ConventionBuilder<ContextType, ResultType>
	{
		readonly Expression<Func<ContextType, bool>> condition;
		readonly Action<ConventionRule<ContextType, ResultType>> add;

		public ConventionBuilder(Expression<Func<ContextType, bool>> condition, Action<ConventionRule<ContextType, ResultType>> add)
		{
			this.condition = condition;
			this.add = add;
		}

		public void By(Expression<Func<ContextType,ResultType>> builder) 
		{ 
			add(new ConventionRule<ContextType, ResultType>(builder, condition)); 
		}
	}
}