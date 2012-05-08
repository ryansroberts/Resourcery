using System;

namespace Resourcery.Configuration
{
	public static class Model
	{
		public static Func<object,bool> IsOfType<T>()
		{
			return m => m != null && typeof (T) == m.GetType();
		}

		public static Func<object,bool> ConformsTo<T>(Func<T,bool> test)
		{
			return m => test((T)m);
		}
        
		public static Func<object,bool> HasProperty(Func<dynamic,object> selector)
		{
			return m => selector((dynamic) m) != null;
		}

       
        
	}
}