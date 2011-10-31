/***
 * game1666proto3: Program.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Runtime.ExceptionServices;

namespace game1666proto3
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
