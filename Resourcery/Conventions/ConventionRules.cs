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
		protected Stack<ConventionRuleDecorator<ContextType, ResultType>> decorators = new Stack<ConventionRuleDecorator<ContextType, ResultType>>(); 

		public Func<ResultType> MatchOne(ContextType context)
		{
			return ApplyDecoratorsTo(context, () =>
			{
					if (mustAlways.condition(context))
						return mustAlways.builder(context);
					return rules.Where(conventionRule => conventionRule.condition(context)).Select(r => r.builder(context))
						.First();
			});
		}

		public Func<IEnumerable<ResultType>> MatchAny(ContextType context) { return () => MatchAnyImplementation(context); }

		IEnumerable<ResultType> MatchAnyImplementation(ContextType context)
		{
			if (mustAlways.condition(context))
				yield return ApplyDecoratorsTo(context, () => mustAlways.builder(context))();
			foreach (var match in rules.Where(conventionRule => conventionRule.condition(context)).Select(r => r.builder(context)))
				yield return match;
		}

		protected Func<ResultType> ApplyDecoratorsTo(ContextType context,Func<ResultType> result)
		{
			return () =>
			{
				var matchingDecorator = decorators.Where(r => r.condition(context)).FirstOrDefault();
				if (matchingDecorator != null)
					return matchingDecorator.builder(context, result());

				return result();
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

		public void AddDecorator(ConventionRuleDecorator<ContextType,ResultType> decorator)
		{
			decorators.Push(decorator);
		}

		public void Add(IEnumerable<ConventionRule<ContextType, ResultType>> rules)
		{
			foreach(var rule in rules) Add(rule);
		}
	}
}