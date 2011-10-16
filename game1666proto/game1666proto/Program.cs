/***
 * game1666: Program.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto
{
	static class Program
	{
		static void Main(string[] args)
		{
			using (Game game = new Game())
			{
				game.Run();
			}
		}
	}
}
