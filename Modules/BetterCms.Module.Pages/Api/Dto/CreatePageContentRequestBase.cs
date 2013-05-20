﻿using System;

namespace BetterCms.Module.Pages.Api.Dto
{
    public abstract class CreatePageContentRequestBase
    {
        private string regionIdentifier;
        private Guid? regionId;

        public Guid PageId { get; set; }

        public string RegionIdentifier
        {
            get
            {
                if (regionId != null)
                {
                    throw new InvalidOperationException("RegionId is set. Can be set only on of these: RegionIdentifier or RegionId.");
                }
                return regionIdentifier;
            }
            set
            {
                regionIdentifier = value;
            }
        }

        public Guid? RegionId
        {
            get
            {
                return regionId;
            }
            set
            {
                if (regionIdentifier != null)
                {
                    throw new InvalidOperationException("RegionIdentifier is set. Can be set only on of these: RegionIdentifier or RegionId.");
                }
                regionId = value;
            }
        }
    }
}