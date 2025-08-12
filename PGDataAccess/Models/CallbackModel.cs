using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class CallbackModel
    {
        [DataMember]
        public bool IsCallbackPosted { get; set; }

        [DataMember]
        public string ReturnUrl { get; set; }

    }
}