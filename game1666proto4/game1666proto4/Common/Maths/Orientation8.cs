/***
 * game1666proto4: Orientation8.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Maths
{
	/// <summary>
	/// This enum represents 2D orientation at discrete 45 degree angles.
	/// </summary>
	enum Orientation8
	{
		XPOS,	// +x
		XPYP,	// +x, +y
		YPOS,	// +y
		XNYP,	// -x, +y
		XNEG,	// -x
		XNYN,	// -x, -y
		YNEG,	// -y
		XPYN	// +x, -y
	}
}
