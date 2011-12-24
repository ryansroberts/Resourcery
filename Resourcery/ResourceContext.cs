using System;

namespace Resourcery
{
	public class ResourceContext
	{
		public readonly Type Type;
		public readonly object Instance;
		public ResourceContext ParentContext;

		public ResourceContext(Type type, object instance)
		{
			Type = type;
			Instance = instance;
		}

		public static ResourceContext From<T>(T model)
		{
			return new ResourceContext(typeof(T),model);
		}
	}
}