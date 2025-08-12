using PGMainService.PGDataAccess;
using System;
using System.Collections.Generic;

namespace PGMainService.Models
{
    [Serializable]
    public class MetadataCommerceItems
    {
        public MetadataCommerceItems()
        {
            this.CommerceItems = new List<OldMetadataCommerceItem>();
        }

        public List<OldMetadataCommerceItem> CommerceItems { get; set; }
    }

    public class OldMetadataCommerceItem {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}