/***
 * game1666proto4: ModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	abstract class ModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly IDictionary<string,string> m_properties;	/// the entity's properties

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity's properties.
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
		/// Constructs an entity directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public ModelEntity(IDictionary<string,string> properties)
		{
			m_properties = Properties;
		}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public ModelEntity(XElement entityElt)
		{
			m_properties = new Dictionary<string,string>();

			foreach(XElement propertyElt in entityElt.Elements("property"))
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
