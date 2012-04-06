/***
 * game1666: TestComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666.Common.Entities.Components
{
	/// <summary>
	/// An instance of this class can be used as a component with which to test entity loading.
	/// We have to put it here rather than in the test project because it will be instantiated
	/// by reflection.
	/// </summary>
	sealed class TestComponent : EntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return StaticGroup; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Test"; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "TestGroup"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a test component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public TestComponent(XElement componentElt)
		{}

		#endregion
	}
}
