/***
 * game1666proto4: ModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents an entity in the game model, e.g. a building.
	/// </summary>
	abstract class ModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The entity's properties.
		/// </summary>
		private readonly IDictionary<string,string> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the entity (if any).
		/// </summary>
		public string Name
		{
			get
			{
				string name;
				Properties.TryGetValue("Name", out name);
				return name;
			}
		}

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
		/// Constructs an entity without any properties.
		/// </summary>
		public ModelEntity()
		{
			m_properties = new Dictionary<string,string>();
		}

		/// <summary>
		/// Constructs an entity directly from a set of properties.
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
				// Look up the name of the property.
				XAttribute nameAttribute = propertyElt.Attribute("name");

				// If the property element has a value attribute, use that. Otherwise, use the text enclosed within the element.
				XAttribute valueAttribute = propertyElt.Attribute("value");
				string value = valueAttribute != null ? valueAttribute.Value : propertyElt.Value.Trim().Replace(" ", "");

				// Provided the property is valid, store it for later use.
				if(nameAttribute != null && value != null)
				{
					m_properties[nameAttribute.Value] = value;
				}
			}
		}

		#endregion
	}
}
