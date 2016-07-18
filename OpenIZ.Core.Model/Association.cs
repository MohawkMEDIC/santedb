﻿/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
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
 * Date: 2016-2-1
 */
using OpenIZ.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using OpenIZ.Core.Model.Interfaces;
using Newtonsoft.Json;
using OpenIZ.Core.Model.EntityLoader;

namespace OpenIZ.Core.Model
{
    /// <summary>
    /// Represents a bse class for bound relational data
    /// </summary>
    /// <typeparam name="TSourceType"></typeparam>
    [XmlType(Namespace = "http://openiz.org/model")]
    public abstract class Association<TSourceType> : IdentifiedData, ISimpleAssociation where TSourceType : IdentifiedData, new()
    {

        /// <summary>
        /// Gets or sets the source entity's key (where the relationship is FROM)
        /// </summary>
        [XmlElement("source"), JsonProperty("source")]
        public virtual Guid? SourceEntityKey { get; set; }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [DataIgnore, XmlIgnore, JsonIgnore, SerializationReference(nameof(SourceEntityKey))]
        public TSourceType SourceEntity
        {
            get { return this.EntityProvider?.Get<TSourceType>(this.SourceEntityKey); }
            set { this.SourceEntityKey = value?.Key;  }
        }


    }
}
