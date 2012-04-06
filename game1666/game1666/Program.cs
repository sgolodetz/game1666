/***
 * game1666: Program.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Runtime.ExceptionServices;

namespace game1666
{
	static class Program
	{
		//#################### INTERNAL STATIC METHODS ####################
		#region

		[HandleProcessCorruptedStateExceptions]
		static void Main(string[] args)
		{
			try
			{
				using(Game game = new Game())
				{
					game.Run();
				}
			}
			catch(Exception) {}
		}

		#endregion
	}
}
