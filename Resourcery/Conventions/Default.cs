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

		public static ConventionRule<ResourceContext, string> RootRelIsAlwaysSelf
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(c => "self",c=> c.ParentContext == null);
			}
		}

		public static ConventionRule<ResourceContext,string> HrefIsTypeName
		{
			get
			{
				return new ConventionRule<ResourceContext, string>(
						c => c.Type.Name.ToLower(), c => true
					);
			}
		}

		public static ConventionRuleDecorator<ResourceContext,string> RootResourceHasAbsoluteUri
		{
			get
			{
				return new ConventionRuleDecorator<ResourceContext, string>(
					   (c,u) => "/" + u,c => c.ParentContext == null
					);
			}
		}


		 static ConventionRule<ResourceAttributeContext,Attribute> AttributeForType<TAttr>()
		{
			return new ConventionRule<ResourceAttributeContext, Attribute>(
					c => new Attribute(c.AttributeName,c.AttributeType,c.AttributeValue.ToString()),
					c => c.AttributeType.Equals(typeof(TAttr))
				);
		}

		static ConventionRule<ResourceAttributeContext,Attribute> EnumerableAttributeForType<TAttr>()
		{
			return new ConventionRule<ResourceAttributeContext, Attribute>(
					c => new Attribute(c.AttributeName, c.AttributeType, ((IEnumerable<TAttr>)c.AttributeValue).Select(a => a.ToString()).ToArray()),
					c => c.AttributeType.GetInterfaces().Any(t => t.IsGenericType && 
						t.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
						t.GetGenericArguments()[0].Equals(typeof(TAttr)))
				);
		}

		public static IEnumerable<ConventionRule<ResourceAttributeContext,Attribute>> SimpleTypeAttributes
		{
			get {
				yield return AttributeForType<int>();
				yield return EnumerableAttributeForType<int>();
				yield return AttributeForType<Int16>();
				yield return AttributeForType<Int32>();
				yield return AttributeForType<Int64>();
				yield return AttributeForType<String>();
				yield return EnumerableAttributeForType<String>();
				yield return AttributeForType<DateTime>();
				yield return AttributeForType<bool>();
				yield return AttributeForType<char>();
				yield return AttributeForType<byte>();
			}
		}

	}
}