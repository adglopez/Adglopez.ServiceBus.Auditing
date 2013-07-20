using System.Runtime.Serialization;

namespace Adglopez.Samples.ServiceBus.Auditing.Services
{
    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public string BusinessProcess { get; set; }

        [DataMember]
        public string Content { get; set; }
    }
}