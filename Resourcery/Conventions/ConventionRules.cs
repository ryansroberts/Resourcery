using System;
using System.Collections.Generic;
using System.Linq;

namespace Resourcery.Conventions
{
	public class ConventionRules<ContextType,ResultType>
	{
		protected Stack<ConventionRule<ContextType, ResultType>> rules = new Stack<ConventionRule<ContextType, ResultType>>();

		public Func<ResultType> Match(ContextType context)
		{
			return () => rules.Where(conventionRule => conventionRule.condition(context)).Select(r => r.builder(context))
			             	.FirstOrDefault();
		}

		public void Add(ConventionRule<ContextType, ResultType> rule)
		{
			rules.Push(rule);
		}

		public ConventionBuilder<ContextType, ResultType> When(Func<ContextType,bool> condition)
		{
			return  new ConventionBuilder<ContextType, ResultType>(condition,Add);
		}

		public ConventionBuilder<ContextType, ResultType> Always() { return When(c => true); }

	}
}