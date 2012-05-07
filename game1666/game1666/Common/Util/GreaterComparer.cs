/***
 * game1666: GreaterComparer.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666.Common.Util
{
	/// <summary>
	/// An instance of this class can be used to compare objects in greater-than order (it's like std::greater in C++).
	/// </summary>
	/// <typeparam name="T">The type of the objects being compared.</typeparam>
	sealed class GreaterComparer<T> : IComparer<T>
	{
		/// <summary>
		/// Compares the two specified objects and returns an integer value indicating their relative ordering.
		/// </summary>
		/// <param name="lhs">The left-hand object.</param>
		/// <param name="rhs">The right-hand object.</param>
		/// <returns>
		/// A -ve value if lhs is before rhs in the greater-than ordering (i.e. if lhs is greater than rhs),
		/// a +ve value if lhs is after rhs in the greater-than ordering (i.e. if lhs is less than rhs),
		/// or 0 otherwise.
		/// </returns>
		public int Compare(T lhs, T rhs)
		{
			return -Comparer<T>.Default.Compare(lhs, rhs);
		}
	}
}
