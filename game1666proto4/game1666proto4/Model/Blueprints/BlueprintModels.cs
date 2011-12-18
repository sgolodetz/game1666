/***
 * game1666proto4: BlueprintModels.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class holds the model references for a given blueprint.
	/// </summary>
	sealed class BlueprintModels : Entity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint model reference set from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the blueprint model reference set's XML representation.</param>
		public BlueprintModels(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### INDEXERS ####################
		/// <summary>
		/// An indexer to look up blueprint model references by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The corresponding blueprint model reference, if any, or null otherwise.</returns>
		public string this[string key]
		{
			get
			{
				string modelReference = null;
				Properties.TryGetValue(key, out modelReference);
				return modelReference;
			}
		}
	}
}
