/***
 * game1666proto4: Blueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a blueprint for building an entity.
	/// </summary>
	abstract class Blueprint : CompositeModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A set of model references that specify which model to use for each game view.
		/// </summary>
		private BlueprintModels m_models;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A set of model references that specify which model to use for each game view.
		/// </summary>
		public BlueprintModels Models { get { return m_models; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public Blueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a set of blueprint models to the blueprint (note that there can be only one set of models).
		/// </summary>
		/// <param name="models">The blueprint models.</param>
		public void AddEntity(BlueprintModels models)
		{
			m_models = models;
		}

		#endregion
	}
}
