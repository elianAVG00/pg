using PGPluginSPS.PGDataAccess;
using System;
using System.Collections.Generic;

namespace PGPluginSPS.Models
{
    [Serializable]
    public class MetadataCommerceItems
    {
        public MetadataCommerceItems()
        {
            CommerceItems = new List<CommerceItemModel>();
        }

        public List<CommerceItemModel> CommerceItems { get; set; }
    }
}