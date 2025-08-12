using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public enum LogTypeModel
    {
        [EnumMember]
        Debug,

        [EnumMember]
        Info,

        [EnumMember]
        Warning,

        [EnumMember]
        Error
    }
}