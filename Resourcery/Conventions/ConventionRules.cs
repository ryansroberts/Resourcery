using System;
using System.Collections.Generic;
using System.Linq;

namespace Resourcery.Conventions
{
	public class ConventionRules<ContextType,ResultType>
	{
		protected ConventionRule<ContextType,ResultType> mustAlways = 
			new ConventionRule<ContextType,ResultType> (c => default(ResultType),c => false); 

		protected Stack<ConventionRule<ContextType, ResultType>> rules = new Stack<ConventionRule<ContextType, ResultType>>();

		public Func<ResultType> Match(ContextType context)
		{
			return () =>
			       {
					   if (mustAlways.condition(context))
							return mustAlways.builder(context);
			       		return rules.Where(conventionRule => conventionRule.condition(context)).Select(r => r.builder(context))
			       			.FirstOrDefault();
			       };
		}

		public void MustAlways(ConventionRule<ContextType, ResultType> rule) { mustAlways = rule; }

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