using System;

namespace Resourcery.FunctionalComposition
{
	public static class ConditionalExtensions
	{
		public static Func<T1, T2> Chain<T1, T2>(this Func<T2> subject, Func<T1, bool> condition)
		{
			return subject.Chain(condition, null);
		}

		 public static Func<T1,T2> Chain<T1,T2>(this Func<T2> subject,Func<T1,bool> condition,Func<T2> outer)
		 {
		 	return (t1) =>
		 	       {
					   if (condition(t1))
						   return subject();
					   return outer == null?default(T2):outer();
		 	       };
		 }

		 public static Func<T1, T2> Chain<T1, T2>(this Func<T1,T2> inner, Func<T1, bool> condition, Func<T2> outer)
		 {
			 return (t1) =>
			 {
				 if (condition(t1))
					 return outer();
				 return inner == null ? default(T2) : inner(t1);
			 };
		 }
	}
}