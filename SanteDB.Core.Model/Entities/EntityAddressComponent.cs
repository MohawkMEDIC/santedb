/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2017-9-1
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
	/// <summary>
	/// A single address component
	/// </summary>
	[XmlType("AddressComponent", Namespace = "http://santedb.org/model"), JsonObject("AddressComponent")]
	public class EntityAddressComponent : GenericComponentValues<EntityAddress>
	{
		/// <summary>
		/// Creates a new address component type.
		/// </summary>
		/// <param name="componentType"></param>
		/// <param name="value"></param>
		public EntityAddressComponent(Guid componentType, String value) : base(componentType, value)
		{
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		public EntityAddressComponent()
		{
		}

		/// <summary>
		/// Gets or sets the component type key
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[XmlElement("type"), JsonProperty("type")]
		[Binding(typeof(AddressComponentKeys))]
		public override Guid? ComponentTypeKey
		{
			get
			{
				return base.ComponentTypeKey;
			}

			set
			{
				base.ComponentTypeKey = value;
			}
		}
	}
}