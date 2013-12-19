﻿using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCMS.Module.LuceneSearch.Models
{
    public class IndexSource : EquatableEntity<IndexSource>, IDeleteableEntity
    {
        public virtual Guid SourceId { get; set; }

        public virtual string Path { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? EndTime { get; set; }
    }
}
