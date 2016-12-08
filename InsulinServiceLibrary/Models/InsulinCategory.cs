using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InsulinServiceLibrary.Models
{
    [DataContract]
    public class InsulinCategory
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int CategoryTypeId { get; set; }
        [DataMember]
        public int Slot { get; set; }
        [DataMember]
        public string SlotTypeKey { get; set; }
        [DataMember]
        public string TypeKey { get; set; }
    }
}