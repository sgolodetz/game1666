/***
 * game1666: ObjectLoader.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;

namespace game1666.Common.Persistence
{
	/// <summary>
	/// An instance of this class specifies an object loader that can be used to perform arbitrary
	/// load actions on objects created by the ObjectPersister.LoadChildObjects method. The primary
	/// use for such a loader is to add the child object to a parent.
	/// </summary>
	sealed class ObjectLoader
	{
		/// <summary>
		/// Any additional arguments that need to be passed to the child object's constructor when it is created.
		/// </summary>
		public object[] AdditionalArguments { get; set; }

		/// <summary>
		/// A filter specifying the types for which this is the appropriate loader.
		/// </summary>
		public Func<Type,bool> CanBeUsedFor { get; set; }

		/// <summary>
		/// A custom load action to perform on the created child object.
		/// </summary>
		public Action<dynamic> Load { get; set; }
	}
}
