/***
 * game1666proto4Test: EntityPersisterTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.IO;
using game1666proto4.Common.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.Entities
{
	[TestClass]
	public sealed class EntityPersisterTest
	{
		[TestMethod]
		public void ParseArray2DTest()
		{
			// Parse an empty 2D array of ints.
			Assert.Equal(EntityPersister.ParseArray2D("[0,0]", Convert.ToInt32), new int[,] {});

			// Parse a 2D array of ints containing one element.
			int[,] arrI = EntityPersister.ParseArray2D("[1,1]23", Convert.ToInt32);
			Assert.Equal(arrI.GetLength(0), 1);
			Assert.Equal(arrI.GetLength(1), 1);
			Assert.Equal(arrI, new int[,]
			{
				{ 23 }
			});

			// Parse a 2D array of ints containing a single row.
			arrI = EntityPersister.ParseArray2D("[3,1]23,9,84", Convert.ToInt32);
			Assert.Equal(arrI.GetLength(0), 1);
			Assert.Equal(arrI.GetLength(1), 3);
			Assert.Equal(arrI, new int[,]
			{
				{ 23, 9, 84 }
			});

			// Parse a 2D array of ints containing multiple rows.
			arrI = EntityPersister.ParseArray2D("[ 2 , 4 ] 7, 8, 51,  17, 10, 51,  24, 12", Convert.ToInt32);
			Assert.Equal(arrI.GetLength(0), 4);
			Assert.Equal(arrI.GetLength(1), 2);
			Assert.Equal(arrI, new int[,]
			{
				{ 7, 8 },
				{ 51, 17 },
				{ 10, 51 },
				{ 24, 12 }
			});

			// Attempt to parse a 2D array of ints with too small a size specification.
			Assert.Throws<InvalidDataException>(() => EntityPersister.ParseArray2D("[1,1]23,9,84", Convert.ToInt32));

			// Attempt to parse a 2D array of ints with too large a size specification.
			Assert.Throws<InvalidDataException>(() => EntityPersister.ParseArray2D("[2,2]23", Convert.ToInt32));

			// Parse a 2D array of strings delimited with a separator other than a comma.
			string[,] arrS = EntityPersister.ParseArray2D("[2,2]Foo;Bar;Blah;Wibble", s => s, ';');
			Assert.Equal(arrS, new string[,]
			{
				{ "Foo", "Bar" },
				{ "Blah", "Wibble" }
			});
		}

		[TestMethod]
		public void ParseListTest()
		{
			// Parse an empty list of ints.
			Assert.Equal(EntityPersister.ParseList("", Convert.ToInt32), new List<int> {});

			// Parse a list of ints containing one element.
			Assert.Equal(EntityPersister.ParseList("23", Convert.ToInt32), new List<int> { 23 });

			// Parse a list of ints containing multiple elements.
			Assert.Equal(EntityPersister.ParseList("1,2,3", Convert.ToInt32), new List<int> { 1, 2, 3 });

			// Attempt to parse a list of ints containing a missing element (note that this should fail in the int case).
			Assert.Throws<FormatException>(() => EntityPersister.ParseList("23,,9", Convert.ToInt32));

			// Attempt to parse a list of ints containing an invalid element.
			Assert.Throws<FormatException>(() => EntityPersister.ParseList("7,s,51", Convert.ToInt32));

			// Parse an empty list of strings.
			Assert.Equal(EntityPersister.ParseList("", s => s), new List<string> {});

			// Parse a list of strings containing one element.
			Assert.Equal(EntityPersister.ParseList("Foo", s => s), new List<string> { "Foo" });

			// Parse a list of strings containing multiple elements.
			Assert.Equal(EntityPersister.ParseList("Foo,Bar,Wibble", s => s), new List<string> { "Foo", "Bar", "Wibble" });

			// Parse a list of strings containing a missing element (note that this should work in the string case).
			Assert.Equal(EntityPersister.ParseList("Foo,,Wibble", s => s), new List<string> { "Foo", "", "Wibble" });

			// Parse a list of decimals delimited with a separator other than a comma.
			Assert.Equal(EntityPersister.ParseList("1.7;1.0;5.1", Convert.ToDecimal, ';'), new List<decimal> { 1.7m, 1.0m, 5.1m });

			// Attempt to parse a list of decimals that uses the wrong separator.
			Assert.Throws<FormatException>(() => EntityPersister.ParseList("2.4,1.2,1.8", Convert.ToDecimal, ';'));
		}
	}
}
