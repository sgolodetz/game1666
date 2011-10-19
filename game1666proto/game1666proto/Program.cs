/***
 * game1666proto: Program.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto
{
	static class Program
	{
		//#################### INTERNAL STATIC METHODS ####################
		#region

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
