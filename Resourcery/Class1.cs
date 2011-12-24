using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Resourcery
{
	public class SimplestPossibleModel
	{
	}

	public class ConventionResult<T>
	{
		public readonly T Result;
		public readonly bool ConventionMatched;

		protected ConventionResult(T result, bool conventionMatched)
		{
			Result = result;
			ConventionMatched = conventionMatched;
		}

		public static Func<T1,T2,ConventionResult<T3>> Wrap<T1,T2,T3>(Func<T1,T2,T3> func)
		{
			return (t1,t2) => new ConventionResult<T3>(func(t1,t2),true);
		}

		public static ConventionResult<T> NotMatched()
		{
			return new ConventionResult<T>(default(T),false);
		}

		public static ConventionResult<T> Matched(T result)
		{
			return new ConventionResult<T>(result,true);
		}
	}

	public class ResourceNameConvention
	{
		protected Func<Type, object, ConventionResult<string>> convention;

		public ResourceNameConvention(Func<Type, object, string> convention)
		{
			this.convention = ConventionResult<string>.Wrap(convention);
		}

		public Func<ConventionResult<string>> CurrySubject(Type t, object instance)
		{
			return () => convention(t, instance);
		}

		public static ResourceNameConvention Default
		{
			get
			{
				return new ResourceNameConvention((t,i) => t.Name);
			}
		}

		public void When(Func<Type,object,bool> condition)
		{
			convention = (t, i) =>
			{
				if (condition(t, i))
				{
					return convention(t,i);
				}
				return ConventionResult<string>.NotMatched();
			};
		}
	}

	public class ResourceNameConventions
	{
		protected IList<ResourceNameConvention> NameConventions = new List<ResourceNameConvention>() 
			{ ResourceNameConvention.Default };

		public Func<string> Match(Type t,object instance)
		{
			return () => NameConventions.Select(c => c.CurrySubject(t, instance)())
				.First(c => c.ConventionMatched).Result;
		}

		public ResourceNameConvention Append(Func<Type, object, string> convention)
		{
			var nextConvention = new ResourceNameConvention(convention);
			NameConventions.Insert(0,nextConvention);
			return nextConvention;
		}
	}

	public class Resource : DynamicObject
	{
		public class ResourceIntrinsics
		{
			readonly Func<string> resourceNameConvention;

			public ResourceIntrinsics(Func<string> resourceNameConvention) {
				this.resourceNameConvention = resourceNameConvention;
			}

			public string Name { get { return resourceNameConvention(); } }
		}

		public ResourceIntrinsics Intrinsics { get; protected set; }

		public Resource(Func<string> nameConvention)
		{
			Intrinsics = new ResourceIntrinsics(nameConvention);
		}
	}

	public class Resourcery
	{
		protected ResourceNameConventions ResourceNameConventions = new ResourceNameConventions();

		public Resource Project<T>(T model)
		{
			return new Resource(ResourceNameConventions.Match(typeof(T),model));	
		}

		public ResourceNameConvention BuildResourceTypeNameUsing(Func<Type, object, string> convention)
		{
			return ResourceNameConventions.Append(convention);
		}
	}
}
