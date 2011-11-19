/***
 * game1666proto4: EntityBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	abstract class EntityBlueprint
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly IDictionary<string,string> m_properties;	/// the blueprint's properties

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the blueprint, e.g. "Dwelling" for a house blueprint.
		/// </summary>
		public string BlueprintName
		{
			get
			{
				return Properties["BlueprintName"];
			}
		}

		/// <summary>
		/// The blueprint's properties.
		/// </summary>
		protected IDictionary<string,string> Properties
		{
			get
			{
				return m_properties;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		/// <returns>The blueprint.</returns>
		public EntityBlueprint(XElement blueprintElt)
		{
			m_properties = new Dictionary<string,string>();
			m_properties["BlueprintName"] = blueprintElt.Attribute("name").Value;

			foreach(XElement propertyElt in blueprintElt.Elements("property"))
			{
				XAttribute nameAttribute = propertyElt.Attribute("name");
				XAttribute valueAttribute = propertyElt.Attribute("value");
				string value = valueAttribute != null ? valueAttribute.Value : propertyElt.Value.Trim().Replace(" ", "");
				if(nameAttribute != null && value != null)
				{
					m_properties[nameAttribute.Value] = value;
				}
			}
		}

		#endregion
	}
}
