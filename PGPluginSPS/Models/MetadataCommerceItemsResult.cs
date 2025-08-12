using System;
using System.Collections.Generic;

namespace PGPluginSPS.Models
{
    [Serializable]
    public class MetadataCommerceItemsResult
    {
        public MetadataCommerceItemsResult()
        {
            this.CommerceItems = new List<CommerceItemResult>();
        }

        public List<CommerceItemResult> CommerceItems { get; set; }
    }
}