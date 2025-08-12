using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class StatusResponseMessageModel
    {
        [DataMember]
        public string ResponseGenericCode { get; set; }

        [DataMember]
        public string ResponseGenericMessage { get; set; }

        [DataMember]
        public string ResponseStatusCode { get; set; }

        [DataMember]
        public string ResponseStatusMessage { get; set; }

        [DataMember]
        public string ResponseExtended { get; set; }
    }
}