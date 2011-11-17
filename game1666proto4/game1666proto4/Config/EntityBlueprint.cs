﻿/***
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

		private IDictionary<string,object> m_properties;	/// the blueprint's properties

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint's properties.
		/// </summary>
		protected IDictionary<string,object> Properties
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
			m_properties = new Dictionary<string,object>();

			foreach(XElement propertyElt in blueprintElt.Elements("property"))
			{
				XAttribute nameAttribute = propertyElt.Attribute("name");
				XAttribute valueAttribute = propertyElt.Attribute("value");
				if(valueAttribute != null)
				{
					m_properties[nameAttribute.Value] = valueAttribute.Value;
				}
			}
		}

		#endregion
	}
}
