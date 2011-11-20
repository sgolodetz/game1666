/***
 * game1666proto4: CompositeModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	abstract class CompositeModelEntity : ModelEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		public CompositeModelEntity()
		{}

		public CompositeModelEntity(IDictionary<string,string> properties)
		:	base(properties)
		{}

		public CompositeModelEntity(XElement entityElt)
		:	base(entityElt)
		{
			foreach(XElement childElt in entityElt.Elements("entity"))
			{
				string childTypename = "game1666proto4." + Convert.ToString(childElt.Attribute("type").Value);
				Type childType = Type.GetType(childTypename);
				if(childType != null)
				{
					AddEntityDynamic(Activator.CreateInstance(childType, childElt));
				}
				else
				{
					throw new InvalidOperationException("No such class: " + childTypename);
				}
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		abstract public void AddEntityDynamic(dynamic entity);

		#endregion
	}
}
