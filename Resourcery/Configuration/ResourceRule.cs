using System;
using System.Collections.Generic;
using System.Linq;
using Resourcery.Model;

namespace Resourcery.Configuration
{
	public static class Model<T> where T : class
	{
		public static EmbedRule Embed<E>(Func<T, E> selector, Func<T, string> namegenerator)
		{
			return new EmbedRule(
				m => IsModelType(m) && selector((T) m) != null,
				(m, r) => r.AddEmbeddedResource(selector(m as T), namegenerator((T) m))
				);
		}

		public static EmbedRule EmbedMany<E>(IEnumerable<E> items, Func<E, string> namegenerator)
		{
			return new EmbedRule(
				m => items.Any(),
				(m, r) =>
				{
					foreach (var embeddedModel in items)
						r.AddEmbeddedResource(embeddedModel, namegenerator(embeddedModel));
				});
		}

		public static EmbedRule EmbedMany<E>(Func<T, IEnumerable<E>> selector, Func<E, string> namegenerator)
		{
			return new EmbedRule(
				m => IsModelType(m) && selector((T) m) != null && selector((T) m).Any(),
				(m, r) =>
				{
					foreach (var embeddedModel in selector((T) m))
						r.AddEmbeddedResource(embeddedModel, namegenerator(embeddedModel));
				});
		}

		public static LinkRule Self(Func<T, string> createlink)
		{
			return new LinkRule(
				IsModelType,
				(m, r) => r.AddLink(createlink((T) m), "self")
				);
		}

		static bool IsModelType(object m) { return (m as T) != null; }

		public static FormRule Post<E>()
		{
			return new FormRule(
				IsModelType,
				(m, r) => r.AddForm(typeof (E), "post")
				);
		}

		public static FormRule Post<E>(Func<T, bool> condition)
		{
			return new FormRule(
				m => IsModelType(m) && condition((T) m),
				(m, r) => r.AddForm(typeof (E), "post")
				);
		}

		public static AttributeRule HasAttributesFromProperties(Func<AttributeRule.AttributeInfo, bool> selector)
		{
			return new AttributeRule(c =>
			                         {
			                         	if (IsModelType(c))
			                         		return typeof (T).GetProperties()
			                         			.Select(p => new AttributeRule.AttributeInfo(c, p))
			                         			.Where(selector);

			                         	return new AttributeRule.AttributeInfo[] {};
			                         });
		}

		public static ResourceRule RelativeLink(Func<T, string> selector, string rel)
		{
			return new RelativeLinkRule(
				IsModelType,
				(m, r) => r.AddLink(r.Parent().Self().Href + selector((T) m), rel)
				);
		}

		public static AttributeRule HasAttribute<E>(Func<T, E> selector, Func<T, string> name)
		{
			return new AttributeRule(
				m =>
				{
					if (m is T)
						return
							new[]
							{
								new AttributeRule.AttributeInfo(name((T) m), selector((T) m))
							};

					return new AttributeRule.AttributeInfo[] {};
				});
		}


		public static AttributeRule HasAttributes(Func<T, IEnumerable<AttributeRule.AttributeInfo>> fromInstance)
		{
			return new AttributeRule(m =>
			                         {
			                         	if (m is T)
			                         		return fromInstance((T) m);

			                         	return new AttributeRule.AttributeInfo[] {};
			                         });
		}
	}




	public abstract class ResourceRule
	{
		static readonly Func<object, bool> defaultpredicate = r => false;
		protected Func<object, bool> match = defaultpredicate;
		protected Action<object, global::Resourcery.Model.Resource> action = (r, m) => { };

		public ResourceRule(Func<object, bool> match, Action<object, global::Resourcery.Model.Resource> action)
		{
			this.match = match;
			this.action = action;
		}

		public ResourceRule() { }


		public void AndPredicate(Func<object, bool> predicate)
		{
			if (match == defaultpredicate)
				match = predicate;
			else
				match = m => predicate(m) && match(m);
		}

		public void OrPredicate(Func<object, bool> predicate)
		{
			if (match == defaultpredicate)
				match = predicate;
			else
				match = m => predicate(m) || match(m);
		}

		public void Action(Action<object, global::Resourcery.Model.Resource> action) { this.action = action; }

		public bool Match(object model) { return match(model); }

		public void Apply(object model, global::Resourcery.Model.Resource resource) { action(model, resource); }
	}

	public class RuleBuilder<TRule> where TRule : ResourceRule, new()
	{
		protected TRule rule = new TRule();

		public RuleBuilder() { this.rule = rule; }

		public RuleBuilder<TRule> Model<TModel>()
		{
			rule.OrPredicate(m => m is TModel);

			return this;
		}

		public RuleBuilder<TRule> When(Func<object, bool> predicate)
		{
			rule.OrPredicate(predicate);

			return this;
		}

		public class ReflectionBuilder
		{
			public RuleBuilder<TRule> rule;

			public ReflectionBuilder(RuleBuilder<TRule> rule) { this.rule = rule; }

			public RuleBuilder<TRule> Build() { return rule; }
		}

		public ReflectionBuilder Expression
		{
			get { return new ReflectionBuilder(this); }
		}


		public ResourceRule Build() { return rule; }
	}

	public class LinkRuleBuilder : RuleBuilder<LinkRule>
	{
		
		public LinkRuleBuilder(Action<LinkRuleContext> linkrequirements)
		{
			var reqs = new LinkRuleContext();
			linkrequirements(reqs);

			rule = new LinkRule();
			rule.Action((o,r) => 
					r.AddLink(reqs.href(o),reqs.rel(o))
				);
		}

		public class LinkRuleContext
		{
			public Func<dynamic, string> rel = d => "self";
			public Func<dynamic, string> href;

			public LinkRuleContext Rel(Func<dynamic, string> rel) { this.rel = rel;
				return this;
			}

			public LinkRuleContext Href(Func<dynamic, string> href)
			{
				this.href = href;
				return this;
			}

			public void Rel(string relliteral) { rel = d => relliteral; }
		}
	}

	public class RuleBuilder<TModel, TRule> : RuleBuilder<TRule> where TRule : ResourceRule, new()
	{
		
	}

	public static class ReflectionBuilderExtensions
	{
		public static RuleBuilder<TRule> Matches<TRule>(this RuleBuilder<TRule>.ReflectionBuilder builder,Func<dynamic,bool> predicate) where TRule : ResourceRule, new()
		{
			builder.rule.When(predicate);
			return builder.Build();
		}
	}


	public static class NewDsl
	{

		public static class For<TModel>
		{
			public static RuleBuilder<TModel,LinkRule> Link
			{
				get { return new RuleBuilder<TModel,LinkRule>(); }
			}
		}
		public static LinkRuleBuilder Link(Action<LinkRuleBuilder.LinkRuleContext> linkconfig)
		{
			return new LinkRuleBuilder(linkconfig);
		}
	}


}