using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace PGDataAccess.Models
{
    [DataContract]
    public class AppConfigModel
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Setting { get; set; }
        [DataMember]
        public string ConfigType { get; set; }
        [DataMember]
        public int? ViewOrderInWebAdmin { get; set; }
        [DataMember]
        public string ViewSpecialBackgroundColor { get; set; }
    }
}