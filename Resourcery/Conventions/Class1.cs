using System;
using System.Collections.Generic;
using System.Linq;

namespace Resourcery.Conventions
{
	public static class Default
	{
		public static ConventionRule<ResourceContext,string> ResourceNameRule
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(c => c.Type.Name,c => true);
			}
		}

	}

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