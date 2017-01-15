﻿using OpenIZ.Persistence.Data.ADO.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIZ.Persistence.Data.ADO.Data.Model.Concepts
{
    /// <summary>
    /// Reference term table
    /// </summary>
    [Table("ref_term_tbl")]
    public class DbReferenceTerm : DbNonVersionedBaseData
    {
        /// <summary>
        /// Gets or sets the primary key
        /// </summary>
        [Column("ref_term_id"), PrimaryKey]
        public override Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the code syste
        /// </summary>
        [Column("cs_id"), ForeignKey(typeof(DbCodeSystem), nameof(DbCodeSystem.Key))]
        public Guid CodeSystemKey { get; set; }

        /// <summary>
        /// Gets or sets the mnemonic
        /// </summary>
        [Column("mnemonic")]
        public String Mnemonic { get; set; }
    }
}
