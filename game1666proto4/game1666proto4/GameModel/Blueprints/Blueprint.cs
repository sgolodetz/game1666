﻿/***
 * game1666proto4: Blueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for an entity.
	/// </summary>
	abstract class Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The type of entity this blueprint can be used to build.
		/// </summary>
		public Type EntityType
		{
			get
			{
				string entityTypename = "game1666proto4." + Properties["EntityType"];
				return Type.GetType(entityTypename);
			}
		}

		/// <summary>
		/// The name of the 3D model for this blueprint.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

		/// <summary>
		/// The name of the blueprint.
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The properties of the blueprint.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; set; }

		#endregion
	}
}
