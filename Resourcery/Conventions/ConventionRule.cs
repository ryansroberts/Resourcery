using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Resourcery.Conventions
{
	public class ConventionRule<ContextType, ResultType>
	{
		Expression<Func<ContextType, ResultType>> builder;
		Expression<Func<ContextType, bool>> condition;
		public ConventionRule(Expression<Func<ContextType, ResultType>> builder, Expression<Func<ContextType, bool>> condition)
		{
			this.builder = builder;
			this.condition = condition;
		}

		public string NarrateCondition() { return condition.ToString(); }
		public string NarrateBuilder() { return builder.ToString(); }

		public Func<ContextType, bool> Condition
		{
			get
			{
				Debug.WriteLine("Evaluate condition: " + NarrateCondition());
				return condition.Compile();
			}
		}

		public Func<ContextType, ResultType> Builder
		{
			get { return builder.Compile(); }
		}
	}

	public class ConventionRuleDecorator<ContextType,ResultType>
	{
		public Func<ContextType, ResultType,ResultType> builder;
		public Func<ContextType, bool> condition;

		public ConventionRuleDecorator(Func<ContextType, ResultType, ResultType> builder, Func<ContextType, bool> condition)
		{
			this.builder = builder;
			this.condition = condition;
		}
	}

}